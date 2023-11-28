using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Map
{
    public List<Node> nodes;
    public List<Point> playerPath = new List<Point>();
    public MapConfig mapConfig;

    public Map(MapConfig mapConfig, List<Node> nodes)
    {
        this.nodes = nodes;
        this.mapConfig = mapConfig;
    }

    /// <summary>
    /// Returns the node of the type boss battle from the list of all nodes of this map.
    /// </summary>
    public Node GetBossNode() => nodes.FirstOrDefault(node => node.nodeType == NodeType.BossBattle);
    
    public Node GetNode(Point point) => nodes.FirstOrDefault(node => node.point.Equals(point));
    
    public float DistanceBetweenFirstAndLastLayers()
    {
        Node bossNode = GetBossNode();
        Node firstLayerNode = nodes.FirstOrDefault(node => node.point.col == 0);

        if (bossNode == null || firstLayerNode == null) return 0f;

        return bossNode.pos.x - firstLayerNode.pos.x;
    }
}