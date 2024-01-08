using System.Collections.Generic;
using System.Linq;
using List;
using Misc;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MapGenerator
{
    private static MapConfig _mapConfig;
    
    private static readonly List<List<Node>> _nodes = new List<List<Node>>();
    private static readonly List<List<Point>> _paths = new List<List<Point>>();
    private static readonly List<NodeType> _randomNodes = new List<NodeType> 
        { NodeType.GRAVEYARD, NodeType.SIMPLE_BATTLE, NodeType.ELITE_BATTLE};
    
    /// <summary>
    /// Returns a map based on a map config.
    /// Generate all nodes by layers and paths between them.
    /// </summary>
    public static Map GetMap(MapConfig mapConfig)
    {
        // exit if map config is null
        if (mapConfig == null)
        {
            Debug.LogError("MAP GENERATOR: map config is null");
            return null;
        }
        
        _mapConfig = mapConfig;
        _nodes.Clear();
        
        GenerateNodes();
        GeneratePaths();
        RandomizeNodesPosition();
        SetUpConnections();
        RemoveCrossConnections();

        // select all nodes with connections
        var nodesList = _nodes
            .SelectMany(nodesRow => nodesRow)
            .Where(node => node.incomingNodes.Count > 0 || node.outgoingNodes.Count > 0)
            .ToList();
        
        // Debug.Log($"MAP GENERATOR: map successfully generated with {nodesList.Count} nodes");
        
        return new Map(nodesList);
    }
    
    /// <summary>
    /// Based on the map config, fill each layer with nodes.
    /// The type of those nodes are randomly generated.
    /// </summary>
    private static void GenerateNodes()
    {
        // for the amount of layer in this map config, create a layer of nodes
        for (int layerIndex = 0; layerIndex < _mapConfig.mapLayers.Count; layerIndex++)
        {
            _nodes.Add(new List<Node>());
            MapLayer layer = _mapConfig.mapLayers[layerIndex];
            
            // for the max map width, create a node
            // all layers are fulfilled here, paths will not pass through all nodes (some of those nodes will stay unused)
            for (int nodeIndex = 0; nodeIndex < _mapConfig.GetGridMaxWidth(); nodeIndex++)
            {
                // get a random node type if the random float is less than the randomize node value in the layer
                NodeType nodeType = Random.Range(0f, 1f) < layer.randomizeNodes ? GetRandomNode() : layer.nodeType;
                
                // create and store the node in the corresponding layer
                _nodes[layerIndex].Add(new Node(nodeType, (int)nodeType, new Point(layerIndex, nodeIndex)));
            }
        }
    }

    /// <summary>
    /// Fill the list paths with newly generated paths. 
    /// </summary>
    private static void GeneratePaths()
    {
        int startingNodeAmount = _mapConfig.startingNodeAmount.GetValue();
        int preBossNodeAmount = _mapConfig.preBossNodeAmount.GetValue();
        List<Point> candidates = new List<Point>();
        Point bossPoint = GetBossPoint();
        
        for (int i = 0; i < _mapConfig.GetGridMaxWidth(); i++)
        {
            candidates.Add(new Point(0, i));
        }

        // find starting point candidates
        candidates.Shuffle();
        var startingCandidates = candidates.Take(startingNodeAmount).ToList();
        
        // find pre boss point candidates
        candidates.Shuffle();
        var preBossCandidates = candidates.Take(preBossNodeAmount).ToList();
        foreach (Point point in preBossCandidates)
        {
            point.col = bossPoint.col - 1;
        }
        
        // generate a number of path equals to the max between startingNodeAmount.GetValue() and preBossNodeAmount.GetValue()
        int pathAmount = Mathf.Max(startingNodeAmount, preBossNodeAmount);

        // foreach path:
        for (int pathIndex = 0; pathIndex < pathAmount; pathIndex++)
        {
            // 1. choose a starting and an ending point based on the number of path
            Point startingPoint = startingCandidates[pathIndex % startingNodeAmount];
            Point preBossPoint = preBossCandidates[pathIndex % preBossNodeAmount];

            // 2. GeneratePath();
            List<Point> path = GeneratePath(startingPoint, preBossPoint);
            
            // 3. add the boss point to the path
            path.Add(bossPoint);
            
            // 4. add the newly created path to the list of all paths
            _paths.Add(path);
        }
    }

    /// <summary>
    /// Returns a point on a furthest layer from the start with a random row position 
    /// </summary>
    private static Point GetBossPoint()
    {
        // take the very last column
        int col = _mapConfig.mapLayers.Count - 1;

        // take a random row based on the map width
        int row = Random.Range(0, _mapConfig.GetGridMaxWidth());

        return new Point(col, row);
    }
    
    /// <summary>
    /// Returns a path as a list of points from a starting node to an ending node
    /// </summary>
    private static List<Point> GeneratePath(Point fromPoint, Point toPoint)
    {
        // add the starting node to the path
        List<Point> path = new List<Point>();
        List<Point> candidates = new List<Point>();
        int rowIndex = fromPoint.row;

        for (int layerIndex = 0; layerIndex < _mapConfig.mapLayers.Count - 2; layerIndex++)
        {
            // from the left to the right, check the up, forward and down next points
            for (int forwardRowIndex = rowIndex - 1; forwardRowIndex < rowIndex + 2; forwardRowIndex++)
            {
                if (forwardRowIndex >= 0 && forwardRowIndex < _mapConfig.GetGridMaxWidth())
                {
                    candidates.Add(new Point(layerIndex, forwardRowIndex));
                }
            }

            // save the chosen one to continue the loop on a further layer
            candidates.Shuffle();
            path.Add(candidates[0]);
            
            candidates.Clear();
        }
        
        // add the ending node to the path
        path.Add(toPoint);
        return path;
    }

    /// <summary>
    /// Randomize the position of all nodes based on the layer and the position of other nodes in the layer. 
    /// </summary>
    private static void RandomizeNodesPosition()
    {
        float layerOffset = 0;
        for (int layerIndex = 0; layerIndex < _mapConfig.mapLayers.Count; layerIndex++)
        {
            // get the distance from the previous layer and to the next one
            float layerDistance = _mapConfig.layerDistance.GetValue();
            if (layerIndex > 0) layerOffset += layerDistance;
            List<Vector2> positions = PoissonDiscSampling.GeneratePoints(_nodes[layerIndex].Count, _mapConfig.nodesDistance, _mapConfig.rejectionSamples);

            // get the lowest and the highest position on y axis
            float positionLowestY = 9999;
            float positionHighestY = -9999;
            for (var index = 0; index < positions.Count; index++)
            {
                if (positions[index].y < positionLowestY) positionLowestY = positions[index].y;
                if (positions[index].y > positionHighestY) positionHighestY = positions[index].y;
            }
            
            // get distance from lowest to highest position
            float distanceLowHighY = Mathf.Abs(positionLowestY - positionHighestY);
            
            // foreach node in this layer, randomize their position based on x (position among other nodes in layer) and y (layer)
            for (int nodeIndex = 0; nodeIndex < _nodes[layerIndex].Count; nodeIndex++)
            {
                // update node position based on offsets
                _nodes[layerIndex][nodeIndex].pos.x = positions[nodeIndex].x + layerOffset;
                _nodes[layerIndex][nodeIndex].pos.y = positions[nodeIndex].y - distanceLowHighY / 2;
                
                // center the boss node
                if (layerIndex == _mapConfig.mapLayers.Count - 1) _nodes[layerIndex][nodeIndex].pos.y = 0;
            }
        }
    }
    
    /// <summary>
    /// Fill the connections of all nodes based on paths 
    /// </summary>
    private static void SetUpConnections()
    {
        foreach (List<Point> path in _paths)
        {
            for (int pointIndex = 0; pointIndex < path.Count - 1; pointIndex++)
            {
                // get the corresponding node of the point n and n+1
                Node node = GetNode(path[pointIndex]);
                Node nextNode = GetNode(path[pointIndex + 1]);

                // add outgoing and incoming node to one another
                node.AddOutgoingNode(nextNode.point);
                nextNode.AddIncomingNode(node.point);
            }
        }
    }

    /// <summary>
    /// Remove randomly some cross connections.
    /// </summary>
    private static void RemoveCrossConnections()
    {
        for (int colIndex = 0; colIndex < _mapConfig.mapLayers.Count; colIndex++)
        {
            for (int rowIndex = 0; rowIndex < _mapConfig.GetGridMaxWidth(); rowIndex++)
            {
                // continue if the current, top, right and top-right node doesn't exist and has no connections with others
                Node node = GetNode(new Point(colIndex, rowIndex));
                if (node == null || node.HasNoConnections()) continue;
                
                Node topNode = GetNode(new Point(colIndex, rowIndex + 1));
                if (topNode == null || topNode.HasNoConnections()) continue;
                
                Node rightNode = GetNode(new Point(colIndex + 1, rowIndex));
                if (rightNode == null || rightNode.HasNoConnections()) continue;
                
                Node topRightNode = GetNode(new Point(colIndex + 1, rowIndex + 1));
                if (topRightNode == null || topRightNode.HasNoConnections()) continue;
                
                // continue if any nodes from outgoing node from current equals to top-right 
                
                
                // continue if any nodes from outgoing node from right equals to top
                
                
                // add direct connections: current→top, right→top-right
                
                
                // based on a random float: remove some connections
                
            }
        }
    }
    
    /// <summary>
    /// Returns a random node stored in random nodes list.
    /// It can return an event, a shop, a merge, a simple battle and a campfire.
    /// </summary>
    private static NodeType GetRandomNode()
    {
        return _randomNodes[Random.Range(0, _randomNodes.Count)];
    }
    
    /// <summary>
    /// Returns the node corresponding to the point input
    /// </summary>
    private static Node GetNode(Point point)
    {
        // exit if out of bounds
        if (point.col >= _nodes.Count) return null;
        if (point.row >= _nodes[point.col].Count) return null;

        return _nodes[point.col][point.row];
    }
}