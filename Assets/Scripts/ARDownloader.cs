using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace ActiveText
{
    public class ARContentsDownloader
    {
        private const string name = "ARContentsDownloader";

        private string url;
        public string error;

        public ARContentsDownloader(string url)
        {
            this.url = url;
        }

        public IEnumerator Download(string zipFileName)
        {
            using (var req = UnityWebRequest.Get(url))
            {
                yield return req.SendWebRequest();
                if (req.isNetworkError || req.isHttpError)
                {
                    Debug.LogError(name + ": Download: Error: " + req.error);
                }
                else
                {
                    var downloadDirectory = Paths.ARContentDownloadPath;
                    if (!System.IO.Directory.Exists(downloadDirectory))
                    {
                        System.IO.Directory.CreateDirectory(downloadDirectory);
                    }
                    var savePath = System.IO.Path.Combine(downloadDirectory, zipFileName);

                    System.IO.File.WriteAllBytes(savePath, req.downloadHandler.data);
                    Debug.Log(name + ": Download: Write success: " + savePath);

                    Unzip(savePath);
                }
            }
        }

        public static void Unzip(string savePath)
        {
            var directory = Paths.ARContentsPath;

            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            try
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(savePath, directory);
                Debug.Log(name + ": ExtractToDirectory " + directory);
            }
            catch (Exception e)
            {
                Debug.LogError("zip ファイルの解凍に失敗: " + e.Message);
                return;
            }

            // mac で作った zip の場合 __MACOSX というディレクトリが作られてしまい
            // 次回同じ zip を解凍したときに上書きできないエラーが発生してしまうので
            // さり気なく消しておく.

            var macOSXPath = System.IO.Path.Combine(directory, "__MACOSX");
            if (System.IO.Directory.Exists(macOSXPath))
            {
                System.IO.Directory.Delete(macOSXPath, true);
            }
        }
    }

}