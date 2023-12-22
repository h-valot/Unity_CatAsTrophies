using System;
using UnityEngine;

public class BattlePawn : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [HideInInspector] public BattlePosition battlePosition;
    public string entityIdLinked;
    public int orderInQueue;

    [Header("SPRITES")]
    public Sprite lock1;
    public Sprite lock2;
    public Sprite lock3;
    public Sprite defaultPawn;

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
        orderInQueue = 0;
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
        if (IsLocked())
        {
            switch (orderInQueue)
            {
                case 0:
                    spriteRenderer.sprite = lock1;
                    break;
                case 1:
                    spriteRenderer.sprite = lock2;
                    break;
                case 2:
                    spriteRenderer.sprite = lock3;
                    break;

            }
            spriteRenderer.transform.localPosition = new Vector3(0f, -0.975f, -0.398f);
            spriteRenderer.transform.localEulerAngles = new Vector3 (-58.562f, 0f, 0f);
            spriteRenderer.transform.localScale = Vector3.one * 0.07f;
        }
        else
        {
            spriteRenderer.sprite = defaultPawn;
            spriteRenderer.transform.localPosition = Vector3.zero;
            spriteRenderer.transform.localEulerAngles = Vector3.zero;
            spriteRenderer.transform.localScale = Vector3.one;
        }
    }
}