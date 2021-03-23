using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace ActiveText
{
    public class ActiveTextController : MonoBehaviour
    {
        public GameObject activeTextTemplate;

        public GameObject sceneRootPrefab;
        public GameObject tutorialPrefab;

        public UnityEvent BeginAR;
        public UnityEvent EndAR;

        private GameObject currentAR;
        private GameObject currentSceneRoot;

        [Serializable]
        public class MessageEvent: UnityEvent<string> {}
        [SerializeField]
        public MessageEvent OnMessage;

        public ContentManager contentManager;

        IEnumerator Start()
        {
            Assert.IsNotNull(contentManager);

            ShowTutorialIfNeeded();
            yield return InitializePreinstalledContent();
            yield return StartARIfCameraAuthorized();
        }

        void Update()
        {

        }

        IEnumerator StartARIfCameraAuthorized()
        {
#if UNITY_IOS
            FindWebCams();

            // AR を開始する前に最初にカメラのパーミッションを取得する
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                StartAR();
            }
            else
            {
                Debug.LogWarning("カメラが許可されていません");
            }
#else
            StartAR();
            yield return null;
#endif
        }

        void FindWebCams()
        {
            foreach (var device in WebCamTexture.devices)
            {
                Debug.Log("Name: " + device.name);
            }
        }

        IEnumerator InitializePreinstalledContent()
        {
            if (PlayerPrefs.GetInt(PrefKeys.IsPreinstalledContentExtracted, 0) == 0)
            {
                yield return contentManager.InstallDemoContent();

                PlayerPrefs.SetInt(PrefKeys.IsPreinstalledContentExtracted, 1);

                // 初回起動時にデモコンテンツを選択状態にする
                var contents = contentManager.GetDownloadedContentUrls();
                if (contents.Length > 0)
                {
                    var contentName = Paths.ContentNameFromUrl(contents[0]);
                    PlayerPrefs.SetString(PrefKeys.SelectedContent, contentName);
                }
            }
        }

        void ShowTutorialIfNeeded()
        {
            if (PlayerPrefs.GetInt(PrefKeys.IsPreinstalledContentExtracted, 0) == 0)
            {
                Instantiate(tutorialPrefab);
            }
        }

        public void ContentSelected()
        {
            StopAR();
            StartCoroutine(StartARIfCameraAuthorized());
        }

        public void StartAR() {
            var selectedContent = PlayerPrefs.GetString(PrefKeys.SelectedContent);

            if (!string.IsNullOrEmpty(selectedContent) && System.IO.File.Exists(Paths.ContentConfigPath(selectedContent)))
            {
                Debug.Log("StartAR with " + selectedContent);
                var template = Instantiate(activeTextTemplate, Vector3.zero, Quaternion.identity);
                template.GetComponent<ActiveTextTemplate>().contentDirName = selectedContent;
                currentAR = template;

                var root = Instantiate(sceneRootPrefab);
                root.GetComponent<SceneRootTemplate>().contentDirName = selectedContent;
                currentSceneRoot = root;
                OnMessage.Invoke("Loading: " + selectedContent);
                BeginAR.Invoke();
            }
            else
            {
                Debug.LogWarning("Invalid SelectedContent: " + selectedContent);

                if (string.IsNullOrEmpty(selectedContent))
                {
                    OnMessage.Invoke("Please select content.");
                }
                else {

                    OnMessage.Invoke("Invalid Content: " + selectedContent);
                }
                PlayerPrefs.DeleteKey(PrefKeys.SelectedContent);
            }
        }

        public void StopAR()
        {
            Debug.Log("StopAR");
            if (currentAR != null)
            {
                var ar = currentAR.GetComponent<ARController>();
                ar.StopAR();
                DestroyImmediate(currentAR);
            }
            if (currentSceneRoot != null) {
                Destroy(currentSceneRoot);
            }
            EndAR.Invoke();
        }
    }
}