using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;
using UnityEngine.Assertions;

namespace ActiveText
{
    public class MainUI : MonoBehaviour
    {
        public GameObject SettingsUIPrefab;
        public GameObject FileListUIPrefab;

        public GameObject[] AR_UI;
        public GameObject[] SettingUI;

        public bool IsRunningAR;

        public TMP_Text messageText;

        public ContentManager contentManager;

        private void Start()
        {
            Assert.IsNotNull(contentManager);
        }

        void Update()
        {
            foreach (var ui in SettingUI)
            {
                ui.SetActive(!IsRunningAR);
            }
            foreach (var ui in AR_UI)
            {
                ui.SetActive(IsRunningAR);
            }
        }

        public void ShowMessage(string msg)
        {
            StartCoroutine(RenderMessage(msg));
        }

        private IEnumerator RenderMessage(string msg)
        {
            messageText.enabled = true;
            messageText.text = msg;
            yield return new WaitForSeconds(5);
            messageText.text = "";
            messageText.enabled = false;
        }

        public void AR_Started()
        {
            IsRunningAR = true;
        }

        public void AR_Stoped()
        {
            IsRunningAR = false;
        }

        public void ShowSettingsUI()
        {
            Instantiate(SettingsUIPrefab);
        }

        public void ShowFileListUI()
        {
            var obj = Instantiate(FileListUIPrefab);
            var fileListUI = obj.GetComponent<FileListUI>();
            fileListUI.contentManager = contentManager;
        }
    }
}