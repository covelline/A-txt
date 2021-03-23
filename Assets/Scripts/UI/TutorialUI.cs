using UnityEngine;

namespace ActiveText
{
    public class TutorialUI : MonoBehaviour
    {
        public void OpenSampleMarkerURL()
        {
            Application.OpenURL("https://covelline.com/a-txt/");
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
