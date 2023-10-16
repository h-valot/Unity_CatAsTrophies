using UnityEngine;

public class Cat : MonoBehaviour
{
    [Header("DEBUGGING")]
    public CatState state;
    public int typeIndex;
    public string id;

    public float health;

    public void Initialize(int _typeIndex)
    {
        state = CatState.InDeck;
        typeIndex = _typeIndex;
        id = Misc.GetRandomId();

        health = Registry.catConfig.cats[typeIndex].health;
    }
    
    public bool CanMove() => state == CatState.InHand;

    /// <summary>
    /// Remove this cat from the cat pool
    /// </summary>
    public void Replace()
    {
        GraveyardManager.Instance.AddCat(this);
        
        // finally repool the cat and desactivating it
        CatGenerator.Instance.Pool(this);
    }
}

public enum CatState
{
    InDeck = 0,
    InHand,
    OnBattle,
    InGraveyard
}