using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;
    
    [Header("REFERENCES")]
    public Transform[] handPoints;
    
    [Header("DEBUGGING")]
    public string[] catsInHand;
    public Cat catSpawned;

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        Instance.catsInHand = new[] {"", "", "", "", ""};
    }

    /// <summary>
    /// Spawn a cat and add it to the player's hand
    /// </summary>
    /// <param name="_catId">Type index of the cat</param>
    public void DrawCat(string _catId)
    {
        if (CatGenerator.Instance.totalCatCount < Registry.playerConfig.deckLenght)
        {
            catSpawned = CatGenerator.Instance.SpawnCatGraphics(Misc.GetCatById(_catId).typeIndex);
        }
        else
        {
            Misc.GetCatById(_catId).PutInHand();
        }
        AddToHand(catSpawned.id);
    }
    
    /// <summary>
    /// Add a cat to the player's hand
    /// </summary>
    public void AddToHand(string _catId)
    {
        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] == "")
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
                catsInHand[i] = "";
            }
        }
    }

    /// <summary>
    /// Discard all cats in the player's hand
    /// </summary>
    public void DiscardHand()
    {
        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] != "")
            {
                Misc.GetCatById(catsInHand[i]).Withdraw();
                RemoveFromHand(catsInHand[i]);
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
            if (catsInHand[i] == "")
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