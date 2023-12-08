using System.Collections.Generic;
using System.Linq;
using Data;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class Map
{
    [Header("DEBUGGING")]
    public List<Node> nodes;
    public List<Point> playerPath = new List<Point>();

    public Map(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    /// <summary>
    /// Returns the node of the type boss battle from the list of all nodes of this map.
    /// </summary>
    public Node GetBossNode() => nodes.FirstOrDefault(node => node.nodeType == NodeType.BOSS_BATTLE);
    
    /// <summary>
    /// Returns a point corresponding to the given node
    /// </summary>
    public Node GetNode(Point point) => nodes.FirstOrDefault(node => node.point.Equals(point));
    
    /// <summary>
    /// Returns the distance between the first and the last layer
    /// </summary>
    public float DistanceBetweenFirstAndLastLayers()
    {
        Node bossNode = GetBossNode();
        Node firstLayerNode = nodes.FirstOrDefault(node => node.point.col == 0);

        if (bossNode == null || firstLayerNode == null) return 0f;

        return bossNode.pos.x - firstLayerNode.pos.x;
    }
    
    /// <summary>
    /// Returns a json converted version of this class
    /// </summary>
    public string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

    /// <summary>
    /// Returns true is there is nodes in the nodes list, otherwise false
    /// </summary>
    public bool IsNotEmpty() => nodes.Count > 0;

    /// <summary>
    /// Remove the last move of the player, only if the player has left the last battle
    /// </summary>
    public void UndoPlayerPath()
    {
        // exit, if the player didn't left the battle
        if (DataManager.data.endBattleStatus != EndBattleStatus.LEFT) return;
        
        playerPath.Remove(playerPath[^1]);
        DataManager.data.endBattleStatus = EndBattleStatus.NONE;
    }
}