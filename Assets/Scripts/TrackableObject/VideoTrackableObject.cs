using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;

namespace ActiveText {
    public class VideoTrackableObject : MonoBehaviour
    {

        /// 動画の高さ 30cm を基準として videoScale で調整できるようにする.
        private const float BASE_HEIGHT = 0.3F;
        [Range(0.0f, 1.0f)]
        public float videoScale = 1;

        [SerializeField]
        private VideoPlayer VideoPlayer;

        [SerializeField]
        private GameObject VideoPlayerQuad;

        [SerializeField]
        private GameObject FullscreenVideoPrefab;

        public string videoURL;
        void Start()
        {
            VideoPlayer.url = videoURL;
        }

        // Update is called once per frame
        void Update()
        {
            if (VideoPlayer.isPrepared)
            {
                float ratio = (float)VideoPlayer.texture.width / (float)VideoPlayer.texture.height;
                var w = BASE_HEIGHT * videoScale * ratio;
                var h = BASE_HEIGHT * videoScale;

                VideoPlayerQuad.transform.localScale = new Vector3(w, h, 1);
            }
        }

        public void OnClickVideo(BaseEventData data)
        {
            VideoPlayer.Stop();

#if !UNITY_EDITOR && UNITY_IOS
            Handheld.PlayFullScreenMovie("file://" + videoURL);
#elif !UNITY_EDITOR && UNITY_ANDROID
            Handheld.PlayFullScreenMovie(videoURL);
#else
            var obj = Instantiate(FullscreenVideoPrefab);
            var video = obj.GetComponent<FullscreenVideo>();
            video.VideoURL = videoURL;
#endif
        }
    }
}
