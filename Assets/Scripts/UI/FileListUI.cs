using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Assertions;

namespace ActiveText
{
    // JsonUtility は root 配列扱えないので workaround.
    // https://forum.unity.com/threads/how-to-load-an-array-with-jsonutility.375735/#post-2588604
    public class JsonHelper
    {
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }

    public class FileListUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject scrollContent;

        public UnityEvent SelectContent;

        public GameObject CellPrefab;

        public GameObject DialogPrefab;

        public TMP_Text MessageText;

        public ContentManager contentManager;

        struct FileListItem
        {
            public string title;
            public bool isChecked;
            public Action onClick;

            public Action onLongClick;
        }

        void Start()
        {
            Assert.IsNotNull(contentManager);
            contentManager.OnProgressMessage = (t) => MessageText.text = t;
            ReloadFileList();
        }

        private void Update()
        {
            MessageText.enabled = !string.IsNullOrEmpty(MessageText.text);
        }

        public void Close()
        {
            Destroy(gameObject);
        }

        private IEnumerator Select(string url)
        {
            yield return contentManager.Select(url);
            Close();
        }

        private void OpenDeleteDialog(string url, string title)
        {
            var obj = Instantiate(DialogPrefab);
            var dialog = obj.GetComponent<Dialog>();

            dialog.SetText("次のコンテンツを削除しますか？: " + title);
            dialog.OnClickOK = () =>
            {
                RemoveSelectingCache(url);
            };
        }

        private IEnumerator LoadFileList()
        {

            var baseURL = PlayerPrefs.GetString(PrefKeys.ServerURL);

            if (string.IsNullOrEmpty(baseURL))
            {
                // TODO: error message if empty.
                yield break;
            }

            UriBuilder listGetURL;

            try
            {
                listGetURL = new UriBuilder(baseURL);
            }
            catch
            {
                Debug.LogWarning($"URL が不正: {baseURL}");
                yield break;
            }

            listGetURL.Path = "list";

            using (var req = UnityWebRequest.Get(listGetURL.ToString()))
            {
                MessageText.text = "Loading...";
                yield return req.SendWebRequest();
                if (req.isNetworkError || req.isHttpError)
                {
                    Debug.LogError(name + ": Download: Error: " + req.error);
                    MessageText.text = "Error: " + req.error;
                }
                else
                {
                    MessageText.text = "";
                    var contents = JsonHelper.getJsonArray<string>(req.downloadHandler.text);
                    var listItems = contents.Select(url => new FileListItem
                    {
                        title = System.IO.Path.GetFileNameWithoutExtension(url),
                        isChecked = contentManager.HasCache(url),
                        onClick = () => StartCoroutine(Select(url)),
                        onLongClick = () => OpenDeleteDialog(url, System.IO.Path.GetFileNameWithoutExtension(url))
                    });

                    foreach (var item in listItems)
                    {
                        if (!item.isChecked)
                        {
                            AddCell(item);
                        }
                    }
                }
            }
        }

        // セルは再利用されない前提で作っているので、
        // 画面を更新するときはかならずすべてのセルを削除して作り直す
        private void UpdateCells(IList<FileListItem> items)
        {
            RemoveAllCells();
            foreach (var item in items)
            {
                AddCell(item);
            }
        }

        private void AddCell(FileListItem item)
        {
            var obj = Instantiate(CellPrefab, scrollContent.transform);
            var cell = obj.GetComponent<FileListCell>();
            cell.Title = item.title;
            cell.isChecked = item.isChecked;
            cell.onClick = item.onClick;
            cell.onLongClick = item.onLongClick;
        }

        private void RemoveAllCells()
        {
            foreach (Transform child in scrollContent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void RemoveSelectingCache(string url)
        {
            contentManager.RemoveSelectingCache(url);
            ReloadFileList();
        }

        private void ReloadFileList()
        {
            // Cache されてるファイルの一覧を作る.
            if (System.IO.Directory.Exists(Paths.ARContentsPath))
            {
                var contents = contentManager.GetDownloadedContentUrls();

                var listItem = contents.Select(url => new FileListItem
                {
                    title = System.IO.Path.GetFileName(url),
                    isChecked = contentManager.HasCache(url),
                    onClick = () => StartCoroutine(Select(url)),
                    onLongClick = () => OpenDeleteDialog(url, System.IO.Path.GetFileName(url))
                }).ToList();

                UpdateCells(listItem);
            }

            StartCoroutine(LoadFileList());
        }
    }
}