using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("REFERENCE")]
    public MapConfig mapConfig;
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

    private void Initialize()
    {
        GenerateNewMap();
    }
    
    /// <summary>
    /// Generates a new map based on the map config
    /// </summary>
    private void GenerateNewMap()
    {
        currentMap = MapGenerator.GetMap(mapConfig);
        mapView.ShowMap(currentMap);
    }

    public void SaveMap()
    {
        
    }
}