using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Map
{
    public List<Node> nodes;
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
}