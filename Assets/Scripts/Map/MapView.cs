using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    public MapConfig mapConfig;
    public GameObject nodePrefab;
    public Transform contentParent;

    private Map map;
    private List<NodeUI> nodePrefabs = new List<NodeUI>();
    
    public void ShowMap(Map map)
    {
        this.map = map;
        
        ShowNodes();
    }

    private void ShowNodes()
    {
        foreach (Node node in map.nodes)
        {
             var newNodeUI = Instantiate(nodePrefab, contentParent).GetComponent<NodeUI>();
            nodePrefabs.Add(newNodeUI);
        }
    }
}