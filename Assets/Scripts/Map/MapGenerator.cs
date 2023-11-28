using System.Collections.Generic;
using System.Linq;
using List;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MapGenerator
{
    private static MapConfig mapConfig;
    private static List<List<Node>> nodes = new List<List<Node>>();
    private static List<List<Point>> paths = new List<List<Point>>();
    
    private static readonly List<NodeType> randomNodes = new List<NodeType> 
        { NodeType.Event, NodeType.Shop, NodeType.Merge, NodeType.SimpleBattle, NodeType.Campfire };
    
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
        
        MapGenerator.mapConfig = mapConfig;
        nodes.Clear();
        
        GenerateNodes();
        GeneratePaths();
        RandomizeNodesPosition();
        SetUpConnections();
        RemoveCrossConnections();

        // select all nodes with connections
        var nodesList = nodes
            .SelectMany(nodesRow => nodesRow)
            .Where(node => node.incomingNodes.Count > 0 || node.outgoingNodes.Count > 0)
            .ToList();
        
        Debug.Log($"MAP GENERATOR: map successfully generated with {nodesList.Count} nodes");
        
        return new Map(MapGenerator.mapConfig, nodesList);
    }
    
    /// <summary>
    /// Based on the map config, fill each layer with nodes.
    /// The type of those nodes are randomly generated.
    /// </summary>
    private static void GenerateNodes()
    {
        // for the amount of layer in this map config, create a layer of nodes
        for (int layerIndex = 0; layerIndex < mapConfig.mapLayers.Count; layerIndex++)
        {
            nodes.Add(new List<Node>());
            MapLayer layer = mapConfig.mapLayers[layerIndex];
            
            // for the max map width, create a node
            // all layers are fulfilled here, paths will not pass through all nodes (some of those nodes will stay unused)
            for (int nodeIndex = 0; nodeIndex < mapConfig.GetGridMaxWidth(); nodeIndex++)
            {
                // get a random node type if the random float is less than the randomize node value in the layer
                NodeType nodeType = Random.Range(0f, 1f) < layer.randomizeNodes ? GetRandomNode() : layer.nodeType;
                
                // get the node config corresponding to the node type
                NodeConfig nodeConfig = mapConfig.nodes.FirstOrDefault(nodeConfig => nodeConfig.nodeType == nodeType);
                
                // create and store the node in the corresponding layer
                nodes[layerIndex].Add(new Node(nodeType, nodeConfig, new Point(layerIndex, nodeIndex)));
            }
        }
    }

    /// <summary>
    /// Fill the list paths with newly generated paths. 
    /// </summary>
    private static void GeneratePaths()
    {
        int startingNodeAmount = mapConfig.startingNodeAmount.GetValue();
        int preBossNodeAmount = mapConfig.preBossNodeAmount.GetValue();
        List<Point> candidates = new List<Point>();
        Point bossPoint = GetBossPoint();
        
        for (int i = 0; i < mapConfig.GetGridMaxWidth(); i++)
        {
            candidates.Add(new Point(0, i));
        }

        // find starting point candidates
        candidates.Shuffle();
        var startingCandidates = candidates.Take(startingNodeAmount).ToList();
        foreach (Point point in startingCandidates)
        {
            point.col = 0;
        }
        
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
            paths.Add(path);
        }
    }

    /// <summary>
    /// Returns a point on a furthest layer from the start with a random row position 
    /// </summary>
    private static Point GetBossPoint()
    {
        // take the very last column
        int col = mapConfig.mapLayers.Count - 1;

        // take a random row based on the map width
        int row = Random.Range(0, mapConfig.GetGridMaxWidth());

        return new Point(col, row);
    }
    
    /// <summary>
    /// Returns a path as a list of points from a starting node to an ending node
    /// </summary>
    private static List<Point> GeneratePath(Point fromPoint, Point toPoint)
    {
        // add the starting node to the path
        List<Point> path = new List<Point> { fromPoint };
        List<Point> candidates = new List<Point>();
        int rowIndex = fromPoint.row;

        for (int layerIndex = 0; layerIndex < mapConfig.mapLayers.Count - 1; layerIndex++)
        {
            // from the left to the right, check the up, forward and down next points
            for (int forwardRowIndex = rowIndex - 1; forwardRowIndex < rowIndex + 1; forwardRowIndex++)
            {
                if (forwardRowIndex >= 0 && forwardRowIndex < mapConfig.GetGridMaxWidth())
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
        for (int layerIndex = 0; layerIndex < mapConfig.mapLayers.Count; layerIndex++)
        {
            // get the distance from the previous layer and to the next one
            float layerDistance = mapConfig.layerDistance.GetValue();
            
            if (layerIndex > 0) layerOffset += layerDistance;
            float distanceFromPreviousLayer = layerOffset - layerDistance;
            float distanceToNextLayer = layerOffset + layerDistance;

            // foreach node in this layer, randomize their position based on x (position among other nodes in layer) and y (layer)
            for (int nodeIndex = 0; nodeIndex < nodes[layerIndex].Count; nodeIndex++)
            {
                float xRnd = Random.Range(-.5f, .5f);
                float yRnd = Random.Range(-.5f, .5f);

                Node node = nodes[layerIndex][nodeIndex];
                node.pos.x = xRnd + layerOffset;
                node.pos.y = yRnd * mapConfig.nodesDistance * nodeIndex + 1;
                if (layerIndex == mapConfig.mapLayers.Count - 1) node.pos.y = 0;
            }
        }
    }
    
    /// <summary>
    /// Fill the connections of all nodes based on paths 
    /// </summary>
    private static void SetUpConnections()
    {
        foreach (List<Point> path in paths)
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
        for (int colIndex = 0; colIndex < mapConfig.mapLayers.Count; colIndex++)
        {
            for (int rowIndex = 0; rowIndex < mapConfig.GetGridMaxWidth(); rowIndex++)
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
        return randomNodes[Random.Range(0, randomNodes.Count)];
    }
    
    /// <summary>
    /// Returns the node corresponding to the point input
    /// </summary>
    private static Node GetNode(Point point)
    {
        // exit if out of bounds
        if (point.col >= nodes.Count) return null;
        if (point.row >= nodes[point.col].Count) return null;

        return nodes[point.col][point.row];
    }
}