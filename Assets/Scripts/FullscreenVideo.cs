using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

namespace ActiveText
{
    public class FullscreenVideo : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        public Button playButton;
        public Button pauseButton;
        public Slider slider;
        public RawImage rawImage;

        private bool isSliderDragging;

        public string VideoURL
        {
            get { return videoPlayer.url; }
            set { 
                videoPlayer.url = value;
                videoPlayer.Prepare();
            }
        }

        // デバッグ用に inspector から VideoURL をセットできるようにする
        [SerializeField]
        private string defaultVideoURL;

        // Start is called before the first frame update
        void Start()
        {
            playButton.onClick.AddListener(videoPlayer.Play);
            pauseButton.onClick.AddListener(videoPlayer.Pause);
            slider.onValueChanged.AddListener((value) =>
            {
                videoPlayer.time = value * videoPlayer.length;
            });
            videoPlayer.prepareCompleted += (source) =>
            {
                var renderTexture = new RenderTexture(
                    (int)source.height,
                    (int)source.width,
                    0
                );
                videoPlayer.targetTexture = renderTexture;
                rawImage.texture = renderTexture;
                videoPlayer.Play();
            };

            if (!string.IsNullOrEmpty(defaultVideoURL))
            {
                VideoURL = defaultVideoURL;
            }
        }

        // Update is called once per frame
        void Update()
        {
            playButton.gameObject.SetActive(!videoPlayer.isPlaying);
            pauseButton.gameObject.SetActive(videoPlayer.isPlaying);

            if (!isSliderDragging)
            {
                var time = (float)(videoPlayer.time / videoPlayer.length);
                slider.SetValueWithoutNotify(time);
            }
        }

        public void OnSliderPointerDown()
        {
            isSliderDragging = true;
        }

        public void OnSliderPointerUp()
        {
            isSliderDragging = false;
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
