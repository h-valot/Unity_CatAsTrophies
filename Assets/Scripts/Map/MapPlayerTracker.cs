using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using List;
using UnityEngine;
using UnityEngine.SceneManagement;
using List = NUnit.Framework.List;
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

        DataManager.data.endBattleStatus = EndBattleStatus.NONE;
        
        switch (nodeUI.node.nodeType)
        {
            case NodeType.BOSS_BATTLE:
                DataManager.data.compoIndexToLoad = GetComposition(CompositionTier.BOSS);
                SceneManager.LoadScene("GameBattle");
                break;
            case NodeType.ELITE_BATTLE:
                DataManager.data.compoIndexToLoad = GetComposition(CompositionTier.ELITE);
                SceneManager.LoadScene("GameBattle");
                break;
            case NodeType.SIMPLE_BATTLE:
                DataManager.data.compoIndexToLoad = GetComposition(CompositionTier.SIMPLE);
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

    /// <summary>
    /// Returns the index of a random composition of the given tier  
    /// </summary>
    private int GetComposition(CompositionTier compositionTier)
    {
        var candidates = new List<int>();
        for (int i = 0; i < Registry.entitiesConfig.compositions.Count; i++)
            if (Registry.entitiesConfig.compositions[i].tier == compositionTier)
                candidates.Add(i);
        
        candidates.Shuffle();
        return candidates[Random.Range(0, candidates.Count)];
    }
}