using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Map
{
    public List<Node> nodes;
    public string configName;

    public Map(string configName, List<Node> nodes)
    {
        this.nodes = nodes;
        this.configName = configName;
    }

    /// <summary>
    /// Returns the node of the type boss battle from the list of all nodes of this map.
    /// </summary>
    public Node GetBossNode() => nodes.FirstOrDefault(node => node.nodeType == NodeType.BossBattle);
}