using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActiveText
{
    /*
     * 一度マーカーを認識したら表示したままにする機能
     * 
     * 認識開始後 DelayUntilHoldSec 時間経過したら ARTrackedObject の
     * secondsToRemainVisible を十分に大きい数字にして出しっぱなしにする.
     * オブジェクトをタップしたら secondsToRemainVisible を 0 にして表示を解除する.
     */
    [RequireComponent(typeof(ARTrackedObject))]
    public class TrackableObjectHolder : MonoBehaviour
    {
        public bool enableHold = false;

        [SerializeField]
        private float DelayUntilHoldSec; // この秒数認識していたら表示したままになる.

        private ARTrackedObject trackedObject;
        private float TrackingTime;

        void Start()
        {
            enableHold = PlayerPrefs.GetInt(PrefKeys.HoldOnDetectMarker) == 1;

            trackedObject = GetComponent<ARTrackedObject>();

            // ARTrackedObject の OnTrackableFound を受け取れるようにする
            trackedObject.eventReceiver = gameObject;
        }
        
        void Update()
        {
        }

        public void CancelHold()
        {
            // リセット
            trackedObject.secondsToRemainVisible = 0;
            TrackingTime = 0;
        }

        // ARTrackedObject が飛ばしてくるイベント
        public void OnTrackableFound()
        {
            TrackingTime = 0;
        }

        public void OnTrackableTracked()
        {
            TrackingTime += Time.deltaTime;

            if (TrackingTime >= DelayUntilHoldSec)
            {
                // hold が有効な場合は十分に長い時間に設定して消えないようにする
                trackedObject.secondsToRemainVisible = enableHold ? 100000 : 0;
            }
        }
    }
}