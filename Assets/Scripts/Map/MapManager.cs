using System.Linq;
using UnityEngine;
using Data;

public class MapManager : MonoBehaviour
{
    [Header("REFERENCE")]
    public MapView mapView;
    
    [Header("DEBUGGING")]
    public Map currentMap;
    
    private void OnEnable()
    {
        if (Registry.events == null) return;
        Registry.events.OnSceneLoaded += Initialize;
    }

    private void OnDisable()
    {
        if (Registry.events == null) return;
        Registry.events.OnSceneLoaded -= Initialize;
    }

    /// <summary>
    /// Generate a new map if the old one, doesn't exists or is completed.
    /// Otherwise, show the current map
    /// </summary>
    public void Initialize()
    {
        if (DataManager.data.map != null && DataManager.data.map.IsNotEmpty())
        {
            Map map = DataManager.data.map;
            
            // generate a new map, if the payer has already reached the boss 
            if (map.playerPath.Any(point => point.Equals(map.GetBossNode().point)))
            {
                GenerateNewMap();
            }
            // load the current map, if player has not reached the boss yet
            else
            {
                currentMap = map;
                mapView.ShowMap(map);
            }
        }
        else
        {
            GenerateNewMap();
        }
    }
    
    /// <summary>
    /// Generates a new map based on the map config
    /// </summary>
    public void GenerateNewMap()
    {
        currentMap = MapGenerator.GetMap(Registry.mapConfig);
        mapView.ShowMap(currentMap);
    }

    /// <summary>
    /// Save the current map into the persistant data
    /// </summary>
    public void SaveMap()
    {
        if (currentMap == null) return;

        DataManager.data.map = currentMap;
        DataManager.Save();
    }
    
    private void OnApplicationQuit()
    {
        SaveMap();
    }
}