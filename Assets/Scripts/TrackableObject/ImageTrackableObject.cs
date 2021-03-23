using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ActiveText {

    public class ImageTrackableObject : MonoBehaviour
    {

        /// 画像の高さ 30cm を基準として imageScale で調整できるようにする.
        private const float BASE_HEIGHT = 0.3F;
        [Range(0.0f, 1.0f)]
        public float imageScale = 1;
        private float imageRatio = 1;

        [SerializeField]
        private GameObject imageObject;
        public Texture2D imageTexture;

        [SerializeField]
        private GameObject fullScreenImagePrefab;

        void Start()
        {
            var image = imageObject.GetComponent<SpriteRenderer>();
            var size = new Rect(0, 0, imageTexture.width, imageTexture.height);
            image.sprite = Sprite.Create(imageTexture, size, new Vector2(0.5f, 0.5f));
            imageRatio = image.sprite.bounds.size.x / image.sprite.bounds.size.y;

            // 当たり判定を画像サイズに合わせる
            var boxCollider = imageObject.GetComponent<BoxCollider>();
            boxCollider.size = image.sprite.bounds.size;
        }

        private void Update() {
            var w = BASE_HEIGHT * imageScale * imageRatio;
            var h = BASE_HEIGHT * imageScale;

            imageObject.transform.localScale = new Vector3(w, h, 1F);
        }

        public void OnClickImage(BaseEventData data)
        {
            var obj = Instantiate(fullScreenImagePrefab);
            var image = obj.GetComponent<FullscreenImage>();
            image.ImageTexture = imageTexture;
        }
    }
}
