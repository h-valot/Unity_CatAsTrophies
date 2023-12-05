using System;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapPlayerTracker : MonoBehaviour
{
    public static MapPlayerTracker Instance;
    
    public MapManager mapManager;
    public MapView mapView;

    private bool _isLocked;
    
    private void Awake() => Instance = this;
    
    public async void EnterNode(NodeUI nodeUI)
    {
        if (_isLocked) return;

        _isLocked = true;
        mapManager.currentMap.playerPath.Add(nodeUI.node.point);
        mapManager.SaveMap();
        
        mapView.SetAttainableNodes();
        mapView.SetLineColors();

        await nodeUI.ShowSelectionAnimation();
        
        switch (nodeUI.node.nodeType)
        {
            case NodeType.BOSS_BATTLE:
                var bossCompositions = Registry.entitiesConfig.compositions.Where(compo => compo.tier == CompositionTier.BOSS).ToList();
                DataManager.data.compoToLoad = bossCompositions[Random.Range(0, bossCompositions.Count)];
                SceneManager.LoadScene("GameBattle");
                break;
            case NodeType.ELITE_BATTLE:
                var eliteCompositions = Registry.entitiesConfig.compositions.Where(compo => compo.tier == CompositionTier.ELITE).ToList();
                DataManager.data.compoToLoad = eliteCompositions[Random.Range(0, eliteCompositions.Count)];
                SceneManager.LoadScene("GameBattle");
                break;
            case NodeType.SIMPLE_BATTLE:
                var simpleCompositions = Registry.entitiesConfig.compositions.Where(compo => compo.tier == CompositionTier.SIMPLE).ToList();
                DataManager.data.compoToLoad = simpleCompositions[Random.Range(0, simpleCompositions.Count)];
                SceneManager.LoadScene("GameBattle");
                break;
            case NodeType.SHOP:
                break;
            case NodeType.MERGE:
                break;
            case NodeType.GRAVEYARD:
                SceneManager.LoadScene("GameGraveyard");
                break;
            case NodeType.EVENT:
                break;
            case NodeType.CAMPFIRE:
                SceneManager.LoadScene("GameBonfire");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        _isLocked = false;
    }
}