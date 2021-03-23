using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenImage : MonoBehaviour
{
    [SerializeField]
    private RawImage image;

    [SerializeField]
    private AspectRatioFitter aspectRatioFitter;

    public Texture ImageTexture
    {
        get { return image.texture; }
        set { 
            image.texture = value;
            aspectRatioFitter.aspectRatio = (float)value.width / (float)value.height;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
