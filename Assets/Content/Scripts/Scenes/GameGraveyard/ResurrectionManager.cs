using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using DG.Tweening;
using UI.GameGraveyard;
using UnityEngine;

public class ResurrectionManager : MonoBehaviour
{
    [Header("EXTERNAL REFERENCES")]
    public MapManager mapManager;
    
    [Header("REFERENCES")]
    public UI.GameGraveyard.ResurrectionManager resurrectionManager;
    
    [Header("INTANTIATE")]
    public CatModel catModel;
    public Transform hidePoint, showPoint;
    public float hidingDuration, showingDuration;

    private int _selectedCandidate;
    private Action<int> _onSelectionChanged;
    private readonly List<Vector2Int> _candidates = new List<Vector2Int>();
    
    public async void Initialize()
    {
        _onSelectionChanged += Select;
        
        GetCandidates();
        resurrectionManager.mapManager = mapManager;
        if (_candidates.Count > 0)
        {
            Select(0);
            resurrectionManager.Initialize(_candidates, _onSelectionChanged);
            resurrectionManager.ShowResurrection();
        }
        else
        {
            await resurrectionManager.ShowEmpty();
        }
    }
    
    private void GetCandidates()
    {
        foreach (var item in DataManager.data.playerStorage.inGameDeck)
        {
            for (int index = 0; index < item.cats.Count; index++)
            {
                // continue, if the cat is alive
                if (!item.cats[index].isDead) continue;

                _candidates.Add(new Vector2Int(item.entityIndex, index));
            }
        }
    }

    private async void Select(int selectedCandidate)
    {
        _selectedCandidate = selectedCandidate;
        await AnimateSelection();
    }

    private async Task AnimateSelection()
    {
        catModel.transform.DOMove(hidePoint.position, hidingDuration).SetEase(Ease.InOutQuad);
        await Task.Delay(Mathf.RoundToInt(1000 * hidingDuration));
        
        catModel.UpdateGraphics(_candidates[_selectedCandidate].x);

        catModel.transform.DOMove(showPoint.position, showingDuration).SetEase(Ease.InOutQuad);
        await Task.Delay(Mathf.RoundToInt(1000 * showingDuration));
    }

    private void OnDisable()
    {
        _onSelectionChanged -= Select;
    }
}