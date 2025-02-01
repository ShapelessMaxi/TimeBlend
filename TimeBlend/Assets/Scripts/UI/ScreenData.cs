using UnityEngine;

public class ScreenData
{
    public string title;
    public GameObject emptyParent;

    public ScreenData(string title, GameObject emptyParent)
    {
        this.title = title;
        this.emptyParent = emptyParent;
    }
}