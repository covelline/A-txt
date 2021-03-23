using ActiveText;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARTrackedObject))]
public class LinkTrackableObject : MonoBehaviour
{
    [SerializeField]
    private TextMesh text;

    [SerializeField]
    private GameObject background;

    [SerializeField]
    private GameObject scaling;

    private MeshRenderer textRendrer;

    private string url;

    public float TextScale
    {
        get { return scaling.transform.localScale.x; }
        set
        {
            scaling.transform.localScale = new Vector3(value, value, value);
        }
    }

    private void Start()
    {
        textRendrer = text.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        // 背景の大きさを文字に合わせる
        background.transform.localScale = new Vector3(
            GetWidth(text) * text.transform.localScale.x, 
            background.transform.localScale.y, 
            background.transform.localScale.z
        );
    }

    public void SetLink(string url, string label)
    {
        this.url = url;
        text.text = label ?? url;
    }

    public void OnClickLink()
    {
        OpenLink();
    }

    void OpenLink()
    {
        if (url != null)
        {
            // ブラウザを開く
            Application.OpenURL(url);
        }
    }

    public static float GetWidth(TextMesh mesh)
    {
        float width = 0;
        foreach (char symbol in mesh.text)
        {
            if (mesh.font.GetCharacterInfo(symbol, out CharacterInfo info, mesh.fontSize, mesh.fontStyle))
            {
                width += info.advance;
            }
        }
        var fixScale = 0.7f; // 微妙にサイズが合わないので調整する
        return width * mesh.characterSize * fixScale;
    }

    public void OnTrackableFound()
    {
        // マーカーを認識したときにリンクを自動的に開く
        if (PlayerPrefs.GetInt(PrefKeys.AutoOpenLink) == 1)
        {
            OpenLink();
        }
    }
}
