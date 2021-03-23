using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ActiveText
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField]
        private InputField serverURLInput;

        [SerializeField]
        private Toggle HoldOnDetectMarkerToggle;

        [SerializeField]
        private Toggle AutoOpenLinkToggle;

        public GameObject LicensePrefab;

        void Start()
        {
            serverURLInput.text = PlayerPrefs.GetString(PrefKeys.ServerURL);
            if (string.IsNullOrEmpty(serverURLInput.text))
            {
                serverURLInput.text = "http://localhost:3000";
            }
            Debug.Log("HoldOnDetectMarkerToggle:" + PlayerPrefs.GetInt(PrefKeys.HoldOnDetectMarker));
            HoldOnDetectMarkerToggle.isOn = PlayerPrefs.GetInt(PrefKeys.HoldOnDetectMarker) == 1;
            AutoOpenLinkToggle.isOn = PlayerPrefs.GetInt(PrefKeys.AutoOpenLink) == 1;
        }

        private void Update()
        {

        }

        public void Close()
        {
            Destroy(gameObject);
        }

        public void OnClickClearCache()
        {
            if (System.IO.Directory.Exists(Paths.ARContentDownloadPath))
            {
                System.IO.Directory.Delete(Paths.ARContentDownloadPath, true);
            }

            if (System.IO.Directory.Exists(Paths.ARContentsPath))
            {
                System.IO.Directory.Delete(Paths.ARContentsPath, true);
            }

            PlayerPrefs.DeleteKey(PrefKeys.SelectedContent);
        }

        public void OnChangeServerURL(string serverURL)
        {
            Debug.Log("OnChangeServerURL: " + serverURL);
            if (string.IsNullOrEmpty(serverURL))
            {

                Debug.Log("OnChangeServerURL: Input is empty.");
                return;
            }
            PlayerPrefs.SetString(PrefKeys.ServerURL, serverURL);
        }

        public void OnChangeHoldOnDetectMarker(bool on)
        {
            Debug.Log("OnChangeHoldOnDetectMarker: " + on);
            PlayerPrefs.SetInt(PrefKeys.HoldOnDetectMarker, on ? 1 : 0);
        }

        public void OnChangeAutoOpenLink(bool on)
        {
            Debug.Log("OnChangeAutoOpenLink: " + on);
            PlayerPrefs.SetInt(PrefKeys.AutoOpenLink, on ? 1 : 0);
        }

        public void OpenFeedbackLink()
        {
            // Feedback 用の Google Form をブラウザで開く
            Application.OpenURL("https://forms.gle/TEPBLQYBoj2jXTSt6");
        }

        public void OpenLicense()
        {
            Instantiate(LicensePrefab);
        }
    }
}