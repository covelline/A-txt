using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActiveText
{
    public class ActiveTextTemplate : MonoBehaviour
    {
        public string contentDirName;

        // Start is called before the first frame update
        void Start()
        {
            var path = Paths.ContentConfigPath(contentDirName);
            var jsonString = System.IO.File.ReadAllText(path);
            var contentConfig = JsonUtility.FromJson<ContentConfig>(jsonString);
            var arController = gameObject.GetComponent<ARController>();
            arController.enabled = false;

            foreach (var marker in contentConfig.markers)
            {
                var art = gameObject.AddComponent<ARTrackable>();

                // ARTrackable が色々持っている property を無視して cfg を直接設定する.
                art.cfg = "nft;" + System.IO.Path.Combine(Paths.ContentDirectory(contentDirName), marker.datasetName);
                art.Tag = marker.datasetName;
                art.Type = ARTrackable.TrackableType.NFT;
                art.NFTDataName = marker.datasetName;

                Debug.Log("Load AR Marker: " + art.cfg);
            }

            arController.enabled = true;
        }
    }
}