using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Point point;
    public NodeType nodeType;
    public List<Point> incomingNodes = new List<Point>();
    public List<Point> outgoingNodes = new List<Point>();
    public Vector2 pos;
    public NodeConfig nodeConfig;

    public Node(NodeType nodeType, NodeConfig nodeConfig, Point point)
    {
        this.nodeType = nodeType;
        this.nodeConfig = nodeConfig;
        this.point = point;
    }

    public void AddIncomingNode(Point point)
    {
        // exit if the incoming node list already has a this node
        if (incomingNodes.Any(element => element.Equals(point))) return;
        
        incomingNodes.Add(point);
    }
    
    public void AddOutgoingNode(Point point)
    {
        // exit if the outgoing node list already has a this node
        if (outgoingNodes.Any(element => element.Equals(point))) return;
        
        outgoingNodes.Add(point);
    }
    
    public void RemoveIncoming(Point point)
    {
        incomingNodes.RemoveAll(element => element.Equals(point));
    }

    public void RemoveOutgoing(Point point)
    {
        outgoingNodes.RemoveAll(element => element.Equals(point));
    }

    /// <summary>
    /// Returns true if the node no incoming nodes neither outgoing nodes 
    /// </summary>
    public bool HasNoConnections() => incomingNodes.Count == 0 && outgoingNodes.Count == 0;
}

public enum NodeType
{
    BossBattle = 0,
    EliteBattle,
    SimpleBattle,
    Shop,
    Merge,
    Graveyard,
    Event,
    Campfire
}