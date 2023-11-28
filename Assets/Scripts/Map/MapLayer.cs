using UnityEngine;

[System.Serializable]
public class MapLayer
{
    [Tooltip("Default node type of this layer. If Randomize Nodes is equals to 0, you will get this node 100% of the time")]
    public NodeType nodeType;
    
    [Tooltip("Chance to get a random node that is different from the default node on this layer")]
    [Range(0f, 1f)] public float randomizeNodes;

    [Tooltip("Minimum and maximum distance with the previous layer")]
    public FloatMinMax distanceFromPreviousLayer;
}