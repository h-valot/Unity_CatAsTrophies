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

    [Header("NODES DISPLAY")]
    [Tooltip("Minimum and maximum distance between layers")]
    public FloatMinMax layerDistance;
    
    [Tooltip("Minimum distance between two node")]
    public float nodesDistance = 1;
    
    [Tooltip("Number of poisson disc sampling point positioning tries. High value = low performance but cleaner distance between points")]
    public int rejectionSamples = 50;
    
    [Tooltip("Node map colors")]
    public Color nodeLockedColor, nodeAttaignableColor, nodeVisitedColor;
    
    [Header("LAYERS")]
    public List<MapLayer> mapLayers = new List<MapLayer>();
    
    /// <summary>
    /// Return the max width size between max startingNodeAmount and max endingPreBossNodeAmount.
    /// </summary>
    public int GetGridMaxWidth() => Mathf.Max(startingNodeAmount.max, preBossNodeAmount.max);
}