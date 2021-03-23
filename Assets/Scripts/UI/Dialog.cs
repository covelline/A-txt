using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ActiveText
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField]
        private Text text;

        public Action OnClickOK;
        public Action OnClickCancel;

        public void SetText(string text)
        {
            this.text.text = text;
        }

        public void OnClickOKButton()
        {
            Dismiss();
            OnClickOK?.Invoke();
        }

        public void OnClickCancelButton()
        {
            Dismiss();
            OnClickCancel?.Invoke();
        }

        public void Dismiss()
        {
            Destroy(gameObject);
        }
    }
}
