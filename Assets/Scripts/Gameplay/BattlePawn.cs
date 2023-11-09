using System;
using UnityEngine;

public class BattlePawn : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [HideInInspector] public BattlePosition battlePosition;
    public string entityIdLinked;

    public Action OnEntityUpdated;
    
    public void Setup(string _entityIdLinked)
    {
        entityIdLinked = _entityIdLinked;
        OnEntityUpdated?.Invoke();
    }

    public void Free()
    {
        entityIdLinked = "";
        OnEntityUpdated?.Invoke();
    }

    public bool IsLocked()
    {
        bool output = false;
        
        if (Misc.GetCatById(entityIdLinked) != null)
        {
            output = Misc.GetCatById(entityIdLinked).isAbilityUsed;
        }

        return output;
    }
    
    private void Update()
    {
        spriteRenderer.color = IsLocked() ? Color.red : Color.white;
    }
}