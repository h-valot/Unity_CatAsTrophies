using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapPlayerTracker : MonoBehaviour
{
    public static MapPlayerTracker Instance;
    
    public MapManager mapManager;
    public MapView mapView;

    private bool isLocked;
    
    private void Awake() => Instance = this;
    
    public async void EnterNode(NodeUI nodeUI)
    {
        if (isLocked) return;

        isLocked = true;
        mapManager.currentMap.playerPath.Add(nodeUI.node.point);
        mapManager.SaveMap();
        
        mapView.SetAttainableNodes();
        mapView.SetLineColors();

        await nodeUI.ShowSelectionAnimation();
        
        switch (nodeUI.node.nodeType)
        {
            case NodeType.BOSS_BATTLE:
                break;
            case NodeType.ELITE_BATTLE:
                break;
            case NodeType.SIMPLE_BATTLE:
                SceneManager.LoadScene("GameBattle");
                break;
            case NodeType.SHOP:
                break;
            case NodeType.MERGE:
                break;
            case NodeType.GRAVEYARD:
                break;
            case NodeType.EVENT:
                break;
            case NodeType.CAMPFIRE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}