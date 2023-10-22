using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;
    
    [Header("REFERENCES")]
    public Transform[] handPoints;
    
    [Header("DEBUGGING")]
    public string[] catsInHand;
    public string newCatId;

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        // hand limit is 5
        Instance.catsInHand = new[] {"empty", "empty", "empty", "empty", "empty"};
    }

    /// <summary>
    /// Spawn a cat and add it to the player's hand
    /// </summary>
    /// <param name="_catId">Type index of the cat</param>
    public void DrawCat(string _catId)
    {
        if (CatGenerator.Instance.totalCatCount < Registry.playerConfig.deckLenght)
        {
            newCatId = CatGenerator.Instance.SpawnCatGraphics(Misc.GetCatById(_catId).typeIndex);
        }
        else
        {
            newCatId = Misc.GetCatById(_catId).PutInHand();
        }
        AddToHand(newCatId);
    }
    
    /// <summary>
    /// Add a cat to the player's hand
    /// </summary>
    public void AddToHand(string _catId)
    {
        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] == "empty")
            {
                catsInHand[i] = _catId;
                break;
            }
        }
    }
    
    /// <summary>
    /// Remove a cat from the player's hand
    /// </summary>
    public void RemoveFromHand(string _catId)
    {
        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] == _catId)
            {
                catsInHand[i] = "empty";
            }
        }
    }

    /// <summary>
    /// Discard all cats in the player's hand
    /// </summary>
    public void DiscardHand()
    {
        foreach (string catId in catsInHand)
        {
            if (catId != "empty")
            {
                Misc.GetCatById(catId).Withdraw();
                RemoveFromHand(catId);
            }
        }
    }

    /// <summary>
    /// Seek available position in the player's hand
    /// </summary>
    public Vector3 GetAvailablePosition()
    {
        Vector3 output = Vector3.one;
        
        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] == "empty")
            {
                output = handPoints[i].position;
                break;
            }
        }

        if (output == Vector3.one)
        {
            Debug.LogError("HAND MANAGER: cats in hand limit reach. can't add cat.", this);
            Debug.Break();
        }
        
        return output;
    }
}