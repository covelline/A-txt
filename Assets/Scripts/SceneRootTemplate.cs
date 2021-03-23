using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ActiveText
{
    public class SceneRootTemplate : MonoBehaviour
    {
        public string contentDirName;

        [SerializeField]
        private GameObject VideoTrackedObjectPrefab;
        [SerializeField]
        private GameObject ImageTrackedObjectPrefab;
        [SerializeField]
        private GameObject LinkTrackedObjectPrefab;
        [SerializeField]
        private GameObject UnsupportedTrackableObjectPrefab;

        // Start is called before the first frame update
        void Start()
        {

            if (string.IsNullOrEmpty(contentDirName)) {
                Debug.LogError(name + ": contentDirName が設定されていない.");
                return;
            }

            var path = Paths.ContentConfigPath(contentDirName);
            var jsonString = System.IO.File.ReadAllText(path);
            var contentConfig = JsonUtility.FromJson<ContentConfig>(jsonString);

            foreach (var marker in contentConfig.markers)
            {
                AddTrackedObject(marker);
            }
        }

        private static bool IsImage(string name)
        {
            string[] exts = { ".jpg", ".jpeg", ".png" };
            return exts.Contains(System.IO.Path.GetExtension(name).ToLower());
        }

        private static bool IsLink(string name)
        {
            return name.StartsWith("http://") || name.StartsWith("https://");
        }

        private void AddTrackedObject(ContentMarker content) {

            Debug.Log(name + ": Load ARTrackedObject: " + content.objName + " for " + content.datasetName);
            GameObject tracked;

            if (IsLink(content.objName))
            {
                tracked = Instantiate(LinkTrackedObjectPrefab);
                var obj = tracked.GetComponent<LinkTrackableObject>();
                var url = content.objName;
                obj.SetLink(url, content.label);
                obj.TextScale = content.scale;
            }
            else if (content.objName.EndsWith(".mp4"))
            {
                tracked = Instantiate(VideoTrackedObjectPrefab);
                var path = Paths.ContentDirectory(contentDirName);
                var videoPath = System.IO.Path.Combine(path, content.objName);
                var videoURL = "file://" + videoPath;
                var obj = tracked.GetComponent<VideoTrackableObject>();
                obj.videoURL = videoPath;
                if (content.scale > 0 && content.scale <= 1)
                {
                    obj.videoScale = content.scale;
                }
                else
                {
                    Debug.LogWarning($"Invalid scale {content.scale}. The scale must be 0 < scale <= 1");
                }
            }
            else if (IsImage(content.objName))
            {
                tracked = Instantiate(ImageTrackedObjectPrefab);
                var path = Paths.ContentDirectory(contentDirName);
                var imagePath = System.IO.Path.Combine(path, content.objName);
                var texture = new Texture2D(2, 2);

                texture.LoadImage(System.IO.File.ReadAllBytes(imagePath)); // auto reaize

                var obj = tracked.GetComponent<ImageTrackableObject>();
                obj.imageTexture = texture;
                if (content.scale > 0 && content.scale <= 1)
                {
                    obj.imageScale = content.scale;
                }
                else
                {
                    Debug.LogWarning($"Invalid scale {content.scale}. The scale must be 0 < scale <= 1");
                }
            }
            else
            {
                Debug.LogWarning("Unsupported file: " + content.objName);
                tracked = Instantiate(UnsupportedTrackableObjectPrefab);
            }

            tracked.transform.parent = transform;
            tracked.GetComponent<ARTrackedObject>().TrackableTag = content.datasetName;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}