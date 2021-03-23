using System;

[Serializable]
public class ContentConfig
{
    public ContentMarker[] markers;
}

[Serializable]
public class ContentMarker
{
    public string datasetName;
    public string objName;
    public float scale = 1.0f;

    // リンク表示時に使われる文字列
    public string label;
}
