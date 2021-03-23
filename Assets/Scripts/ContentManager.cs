using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

namespace ActiveText
{
    // コンテンツの管理を行うクラス
    public class ContentManager : MonoBehaviour
    {
        public UnityEvent SelectContent;
        public Action<String> OnProgressMessage;

        public IEnumerator Select(string url)
        {
            if (!HasCache(url))
            {
                var zipName = Paths.CreateZipNameFromUrl(url);
                var donwloader = new ARContentsDownloader(url);
                Debug.Log(name + ": Begin Download: " + url);
                OnProgressMessage.Invoke("Download ....");
                yield return donwloader.Download(zipName);
                Debug.Log(name + ": End Download: " + url);
                OnProgressMessage.Invoke(donwloader.error);
            }
            else
            {
                Debug.Log(name + ": Use Cache for " + url);
            }
            var contentName = Paths.ContentNameFromUrl(url);

            PlayerPrefs.SetString(PrefKeys.SelectedContent, contentName);
            SelectContent.Invoke();
        }

        public void RemoveSelectingCache(string url)
        {
            var zipFileName = Paths.CreateZipNameFromUrl(url);
            if (!zipFileName.EndsWith(".zip"))
            {
                zipFileName += ".zip";
            }
            var zipPath = System.IO.Path.Combine(Paths.ARContentDownloadPath, zipFileName);

            if (System.IO.File.Exists(zipPath))
            {
                Debug.Log("Delete zip file: " + zipPath);
                System.IO.File.Delete(zipPath);
            }
            else
            {
                Debug.Log(zipPath + "Not found");
            }

            var contentPath = Paths.ContentDirectoryFromUrl(url);
            if (System.IO.Directory.Exists(contentPath))
            {
                Debug.Log("Delete zip file: " + contentPath);
                System.IO.Directory.Delete(contentPath, true);
            }

            if (PlayerPrefs.GetString(PrefKeys.SelectedContent) == Paths.ContentNameFromUrl(url))
            {
                PlayerPrefs.DeleteKey(PrefKeys.SelectedContent);
            }
        }

        public bool HasCache(string url)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(url);
            var path = Paths.ContentDirectory(name);
            return System.IO.Directory.Exists(path);
        }

        public string[] GetDownloadedContentUrls()
        {
            var contents = System.IO.Directory.GetDirectories(Paths.ARContentsPath);

            return Array.FindAll(contents, dir =>
                System.IO.File.Exists(Paths.ContentConfigPath(System.IO.Path.GetFileNameWithoutExtension(dir)))
            );
        }

        public IEnumerator InstallDemoContent()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            var url = Paths.PreinstalledContentPath;
            var downloader = new ARContentsDownloader(url);

            yield return downloader.Download(Paths.CreateZipNameFromUrl(url));
#else
            ARContentsDownloader.Unzip(Paths.PreinstalledContentPath);
            return null;
#endif
        }
    }
}
