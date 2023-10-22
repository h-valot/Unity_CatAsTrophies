using System;
using UnityEngine;

public class BattlePawn : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [HideInInspector] public string catIdLinked = "";

    public void Setup(string _catIdLinked)
    {
        catIdLinked = _catIdLinked;
    }

    public bool IsLocked()
    {
        return Misc.GetCatById(catIdLinked).isAbilityUsed;
    }
    
    private void Update()
    {
        spriteRenderer.color = IsLocked() ? Color.red : Color.white;
    }
}