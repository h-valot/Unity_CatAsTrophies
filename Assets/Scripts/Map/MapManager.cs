using System.Linq;
using UnityEngine;
using Data;
using Player;

public class MapManager : MonoBehaviour
{
    [Header("REFERENCE")]
    public MapView mapView;
    public GameObject mapCanvasParent;
    
    [Header("DEBUGGING")]
    public Map currentMap;

    public void DisplayCanvas()
    {
        mapCanvasParent.SetActive(true);
        Initialize();
    }

    public void HideCanvas()
    {
        mapCanvasParent.SetActive(false);
    }

    /// <summary>
    /// Generate a new map if the old one, doesn't exists or is completed.
    /// Otherwise, show the current map
    /// </summary>
    private void Initialize()
    {
        if (DataManager.data.map != null && DataManager.data.map.IsNotEmpty())
        {
            Map map = DataManager.data.map;
            
            // generate a new map, if the payer has already reached the boss 
            if (map.playerPath.Any(point => point.Equals(map.GetBossNode().point)) || 
                DataManager.data.endBattleStatus == EndBattleStatus.DEFEATED)
            {
                GenerateNewMap();
            }
            // load the current map, if player has not reached the boss yet
            else
            {
                map.UndoPlayerPath();
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
        DataManager.data.collection ??= new Collection();
        DataManager.data.collection.SwitchToInGameDeck();
        
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