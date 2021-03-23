using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileListCell : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private Image checkmark;

    public Action onClick = () => { };
    public Action onLongClick = () => { };

    public string Title
    {
        get { return text.text; }
        set { text.text = value; }
    }

    public bool isChecked
    {
        get { return checkmark.enabled; }
        set { checkmark.enabled = value; }
    }

    public void OnClick()
    {
        onClick();
    }

    public void OnLongClick()
    {
        onLongClick();
    }
}
