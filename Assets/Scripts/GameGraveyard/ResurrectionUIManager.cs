using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResurrectionUIManager : MonoBehaviour
{
    [Header("REFERENCES")]
    public GameObject resurrectionParent;
    public GameObject doneParent;
    public GameObject emptyParent;
    
    [Header("RESURRECTION PANEL")] 
    public TextMeshProUGUI catNameTM;
    public GameObject leftButtonObject, rightButtonObject;

    [Header("DONE PANEL")]
    public TextMeshProUGUI doneFlavorTM;
    public float doneDuration = 3f;
    public Image doneLoadingImage;

    [Header("EMPTY PANEL")] 
    public float emptyDuration = 3f;
    public Image emptyLoadingImage;
    
    public MapManager mapManager;
    
    private int _selectedCandidate;
    private Action<int> _onSelectionChanged;
    private List<Vector2Int> _candidates = new List<Vector2Int>();
    
    public void Initialize(List<Vector2Int> candidates, Action<int> onSelectionChanged)
    {
        _candidates = candidates;
        _onSelectionChanged = onSelectionChanged;
        
        UpdateGraphics();
        HideAll();
    }

    public void NavigateToRight()
    {
        _selectedCandidate++;
        if (_selectedCandidate >= _candidates.Count - 1) _selectedCandidate = _candidates.Count - 1;
        _onSelectionChanged?.Invoke(_selectedCandidate);
        UpdateGraphics();
    }
    
    public void NavigateToLeft()
    {
        _selectedCandidate--;
        if (_selectedCandidate <= 0) _selectedCandidate = 0;
        _onSelectionChanged?.Invoke(_selectedCandidate);
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        catNameTM.text = Registry.entitiesConfig.cats[_candidates[_selectedCandidate].x].entityName.ToUpper();
        
        leftButtonObject.SetActive(true);
        rightButtonObject.SetActive(true);
        if (_selectedCandidate <= 0) leftButtonObject.SetActive(false);
        if (_selectedCandidate >= _candidates.Count - 1) rightButtonObject.SetActive(false);
        
    }
    
    public async void Resurrect()
    {
        DataManager.data.playerStorage.inGameDeck.FirstOrDefault(item => item.entityIndex == _candidates[_selectedCandidate].x)!.cats[_candidates[_selectedCandidate].y].Reset();
        await AnimateEnd();
    }

    private async Task AnimateEnd()
    {
        doneFlavorTM.text = $"Your {Registry.entitiesConfig.cats[_candidates[_selectedCandidate].x].entityName.ToLower()} has been resurrected.";
        ShowDone();
        doneLoadingImage.DOFillAmount(1, doneDuration);
        await Task.Delay(Mathf.RoundToInt(1000 * doneDuration));
        
        mapManager.ShowCanvasLocked();
        HideAll();
    }

    private void HideAll()
    {
        HideDone();
        HideEmpty();
        HideResurrection();
    }
    
    public void HideResurrection() => resurrectionParent.SetActive(false);
    public void ShowResurrection() => resurrectionParent.SetActive(true);
    
    public void HideEmpty() => emptyParent.SetActive(false);
    public async Task ShowEmpty()
    {
        emptyParent.SetActive(true);
        emptyLoadingImage.DOFillAmount(1, emptyDuration);
        await Task.Delay(Mathf.RoundToInt(1000 * emptyDuration));
        
        mapManager.ShowCanvasLocked();
        HideAll();
    }
    
    public void HideDone() => doneParent.SetActive(false);
    public void ShowDone() => doneParent.SetActive(true);
}