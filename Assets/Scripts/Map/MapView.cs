using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MapView : MonoBehaviour
{
    [Header("REFERENCES")]
    public GameObject nodePrefab;
    public Transform contentParent;
    public ScrollRect scrollRect;

    [Header("MAP UI SETTINGS")] 
    
    [Header("Nodes settings")]
    public float unitsToPixelsMultiplier = 60f;
    public float verticalOffset;
    public float padding = 400f;
    
    [Header("Lines settings")]
    public UILineRenderer uiLinePrefab;
    public int linePointsCount = 10;
    public float offsetFromNodes = 0.5f;
    public Color lineLockedColor, lineVisitedColor;
    
    [Header("Background settings")]
    public Sprite background;
    public Color32 backgroundColor = Color.white;
    public Vector2 backgroundPadding;
    
    private Map map;
    private List<NodeUI> nodesUI = new List<NodeUI>();
    private readonly List<LineConnection> _lineConnections = new List<LineConnection>();
    
    public void ShowMap(Map currentMap)
    {
        // exit if map config is null
        if (Registry.mapConfig == null)
        {
            Debug.LogError("MAP VIEW: map config is null");
            return;
        }
        
        this.map = currentMap;
        
        ClearMap();
        
        CreateMapParent();
        CreateNodes(map.nodes);
        DrawLines();
        ResetNodesRotation();
        SetAttainableNodes();
        SetLineColors();
        CreateMapBackground();
    }

    private void ClearMap()
    {
        scrollRect.gameObject.SetActive(false);
        nodesUI.Clear();

        while (contentParent.childCount > 0) 
        {
            DestroyImmediate(contentParent.GetChild(0).gameObject);
        }
    }
    
    private void CreateNodes(List<Node> nodes)
    {
        foreach (Node node in nodes)
        {
            var newNodeUI = Instantiate(nodePrefab, contentParent).GetComponent<NodeUI>();
            newNodeUI.Initialize(node);
            newNodeUI.transform.localPosition = GetNodePosition(node);
            nodesUI.Add(newNodeUI);
        }
    }
    
    private Vector2 GetNodePosition(Node node)
    {
        var length = padding + map.DistanceBetweenFirstAndLastLayers() * unitsToPixelsMultiplier;
        return new Vector2((padding - length) / 2f, -backgroundPadding.y / 2f - verticalOffset) + node.pos * unitsToPixelsMultiplier ;
    }
    
    private void CreateMapParent()
    {
        scrollRect.gameObject.SetActive(true);
        
        // set map length
        RectTransform rectTransform = scrollRect.content;
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.x = padding + map.DistanceBetweenFirstAndLastLayers() * unitsToPixelsMultiplier;
        rectTransform.sizeDelta = sizeDelta;
        
        // scroll to origin
        scrollRect.normalizedPosition = Vector2.zero;
    }
    
    private void DrawLines()
    {
        foreach (NodeUI nodeUI in nodesUI)
        {
            foreach (Point connection in nodeUI.node.outgoingNodes)
            {
                AddLineConnection(nodeUI, GetNodeUI(connection));
            }
        }
    }

    private NodeUI GetNodeUI(Point p)
    {
        return nodesUI.FirstOrDefault(nodeUI => nodeUI.node.point.Equals(p));
    }
    
    private void AddLineConnection(NodeUI from, NodeUI to)
    {
        if (uiLinePrefab == null) return;

        UILineRenderer lineRenderer = Instantiate(uiLinePrefab, contentParent.transform);
        lineRenderer.transform.SetAsFirstSibling();
        RectTransform fromRectTransform = from.transform as RectTransform;
        RectTransform toRectTransform = to.transform as RectTransform;
        Vector2 fromPoint = fromRectTransform.anchoredPosition + (toRectTransform.anchoredPosition - fromRectTransform.anchoredPosition).normalized * offsetFromNodes;
        Vector2 toPoint = toRectTransform.anchoredPosition + (fromRectTransform.anchoredPosition - toRectTransform.anchoredPosition).normalized * offsetFromNodes;

        // drawing lines in local space
        lineRenderer.transform.position = from.transform.position + (Vector3) (toRectTransform.anchoredPosition - fromRectTransform.anchoredPosition).normalized * offsetFromNodes;

        // line renderer with 2 points only does not handle transparency properly:
        List<Vector2> list = new List<Vector2>(); 
        for (var i = 0; i < linePointsCount; i++)
        {
            list.Add(Vector3.Lerp(Vector3.zero, toPoint - fromPoint + 2 * (fromRectTransform.anchoredPosition - toRectTransform.anchoredPosition).normalized * offsetFromNodes, (float) i / (linePointsCount - 1)));
        }
        
        // Debug.Log($"MAP VIEW: from {fromPoint} to {toPoint} last point {list[^1]}");
        
        lineRenderer.Points = list.ToArray();
        _lineConnections.Add(new LineConnection(lineRenderer, from, to));
    }

    private void ResetNodesRotation()
    {
        foreach (NodeUI nodeUI in nodesUI)
        {
            nodeUI.transform.rotation = Quaternion.identity;
        }
    }
    
    public void SetAttainableNodes()
    {
        // first set all the nodes as unattainable/locked:
        foreach (NodeUI nodeUI in nodesUI)
        {
            nodeUI.SetState(NodeStates.LOCKED);
        }

        if (map.playerPath.Count == 0)
        {
            // we have not started traveling on this map yet, set entire first layer as attainable:
            foreach (NodeUI nodeUI in nodesUI.Where(nodeUI => nodeUI.node.point.col == 0))
            {
                nodeUI.SetState(NodeStates.ATTAIGNABLE);
            }
        }
        else
        {
            // we have already started moving on this map, first highlight the path as visited:
            foreach (Point point in map.playerPath)
            {
                NodeUI nodeUI = GetNodeUI(point);
                if (nodeUI != null) nodeUI.SetState(NodeStates.VISITED);
            }

            Point currentPoint = map.playerPath[^1];
            Node currentNode = map.GetNode(currentPoint);

            // set all the nodes that we can travel to as attainable:
            foreach (Point point in currentNode.outgoingNodes)
            {
                NodeUI nodeUI = GetNodeUI(point);
                if (nodeUI != null)
                    nodeUI.SetState(NodeStates.ATTAIGNABLE);
            }
        }
    }
    
    public void SetLineColors()
    {
        // set all lines to grayed out first:
        foreach (var connection in _lineConnections)
            connection.SetColor(lineLockedColor);

        // set all lines that are a part of the path to visited color:
        // if we have not started moving on the map yet, leave everything as is:
        if (map.playerPath.Count == 0)
            return;

        // in any case, we mark outgoing connections from the final node with visible/attainable color:
        Point currentPoint = map.playerPath[^1];
        Node currentNode = map.GetNode(currentPoint);

        foreach (var point in currentNode.outgoingNodes)
        {
            var lineConnection = _lineConnections.FirstOrDefault(connection => connection.from.node == currentNode && connection.to.node.point.Equals(point));
            lineConnection?.SetColor(lineVisitedColor);
        }

        if (map.playerPath.Count <= 1) return;

        for (var i = 0; i < map.playerPath.Count - 1; i++)
        {
            Point current = map.playerPath[i];
            Point next = map.playerPath[i + 1];
            var lineConnection = _lineConnections.FirstOrDefault(connection => connection.from.node.point.Equals(current) && connection.to.node.point.Equals(next));
            lineConnection?.SetColor(lineVisitedColor);
        }
    }
    
    private void CreateMapBackground()
    {
        GameObject backgroundObject = new GameObject("Background");
        backgroundObject.transform.SetParent(contentParent.transform);
        backgroundObject.transform.localScale = Vector3.one;
        
        RectTransform rectTransform = backgroundObject.AddComponent<RectTransform>();
        Stretch(rectTransform);
        rectTransform.SetAsFirstSibling();
        rectTransform.sizeDelta = backgroundPadding;
        
        Image image = backgroundObject.AddComponent<Image>();
        image.color = backgroundColor;
        image.type = Image.Type.Sliced;
        image.sprite = background;
    }
    
    private void Stretch(RectTransform rectTransform)
    {
        rectTransform.localPosition = Vector3.zero;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;
    }
}