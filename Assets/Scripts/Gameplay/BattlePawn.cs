using UnityEngine;

public class BattlePawn : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [HideInInspector] public string entityIdLinked = "empty";

    public void Setup(string _entityIdLinked)
    {
        entityIdLinked = _entityIdLinked;
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