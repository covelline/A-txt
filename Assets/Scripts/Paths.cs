using System.Collections;
using System.IO;
using UnityEngine;
namespace ActiveText
{
    public static class Paths
    {
        // zip ファイルの展開先
        public static readonly string ARContentsPath = Path.Combine(Application.persistentDataPath, "downloads");

        // zip ファイルのダウンロード先
        public static readonly string ARContentDownloadPath = Path.Combine(Application.persistentDataPath, "tmp");

        public static string PreinstalledContentPath
        {
            get
            {
                return Path.Combine(Application.streamingAssetsPath, "Demo.zip");
            }
        }

        public static string ContentDirectory(string ContentName) {
            return Path.Combine(ARContentsPath, ContentName);
        }

        public static string ContentConfigPath(string ContentName)
        {
            return Path.Combine(ARContentsPath, ContentName, "config.json");
        }

        public static string CreateZipNameFromUrl(string url)
        {
            return Path.GetFileName(url);
        }

        public static string ContentNameFromUrl(string url)
        {
            return Path.GetFileNameWithoutExtension(url);
        }

        public static string ContentDirectoryFromUrl(string url) {
            return ContentDirectory(ContentNameFromUrl(url));
        }
    }
}