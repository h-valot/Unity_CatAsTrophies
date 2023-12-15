using System;
using UnityEngine;

public class ResurrectionUICard : MonoBehaviour
{
    [Header("REFERENCES")]
    public GameObject graphicsParent;

    private int _id;
    private Action<int> _onSelected;

    public void Initialize(int newId, Action<int> onSelected)
    {
        _id = newId;
        _onSelected = onSelected;
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        
    }

    public void Select()
    {
        _onSelected?.Invoke(_id);
    }
}