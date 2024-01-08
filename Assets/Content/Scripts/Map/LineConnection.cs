using UnityEngine;
using UnityEngine.UI.Extensions;

[System.Serializable]
public class LineConnection
{
    public UILineRenderer uiLineRenderer; 
    public NodeUI from;
    public NodeUI to;

    public LineConnection(UILineRenderer uiLineRenderer, NodeUI from, NodeUI to)
    {
        this.uiLineRenderer = uiLineRenderer;
        this.from = from;
        this.to = to;
    }

    public void SetColor(Color color)
    {
        if (uiLineRenderer != null) uiLineRenderer.color = color;
    }
}