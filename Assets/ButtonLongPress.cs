using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ActiveText
{
    public class ButtonLongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        [SerializeField]
        private UnityEvent OnClick;

        [SerializeField]
        private UnityEvent OnLongClick;

        [SerializeField]
        private float RequiredHoldTimeSec = 1;

        private bool isPointerDown = false;
        private float pointerDownTimer = 0;

        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {

            InvokeOnClickIfNeeded();
            ResetClick();
        }

        void Start()
        {

        }

        void Update()
        {
            if (isPointerDown)
            {
                pointerDownTimer += Time.deltaTime;
                if (pointerDownTimer >= RequiredHoldTimeSec)
                {
                    if (OnLongClick != null)
                    {
                        OnLongClick.Invoke();
                    }
                    ResetClick();
                }
            }
        }

        private void InvokeOnClickIfNeeded()
        {
            if (isPointerDown && pointerDownTimer < RequiredHoldTimeSec)
            {
                if (OnClick != null)
                {
                    OnClick.Invoke();
                }
            }
        }

        private void ResetClick()
        {
            isPointerDown = false;
            pointerDownTimer = 0;
        }
    }
}