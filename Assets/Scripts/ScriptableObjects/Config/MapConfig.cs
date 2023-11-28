using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "Config/Map/Map", order = 1)]
public class MapConfig : ScriptableObject
{
    public List<NodeConfig> nodes = new List<NodeConfig>();

    [Header("MAP SIZE")]
    [Tooltip("Minimum and maximum number of starting node (in the very first layer)")]
    public IntMinMax startingNodeAmount;
    
    [Tooltip("Minimum and maximum number of node that can spawn before the boss")]
    public IntMinMax preBossNodeAmount;

    [Tooltip("Minimum and maximum distance between layers")]
    public FloatMinMax layerDistance;
    
    [Tooltip("Distance between nodes on the same layer")]
    public float nodesDistance;
    
    [Header("LAYERS")]
    public List<MapLayer> mapLayers = new List<MapLayer>();
    
    /// <summary>
    /// Return the max width size between max startingNodeAmount and max endingPreBossNodeAmount.
    /// </summary>
    public int GetGridMaxWidth() => Mathf.Max(startingNodeAmount.max, preBossNodeAmount.max);
}