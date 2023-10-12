using System;
using UnityEngine;

public class BattlePawn : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [HideInInspector] public bool isOccupied;
    [HideInInspector] public string catIdLinked = "";

    public void Setup(string _catIdLinked)
    {
        isOccupied = true;
        catIdLinked = _catIdLinked;
    }
    
    private void Update()
    {
        if (isOccupied) return;
        
        spriteRenderer.color = 
            BattlePawnManager.Instance.IsCloseEnough(transform.position, InputHandler.Instance.touchPos) 
                ? Color.yellow 
                : Color.white;
    }
}