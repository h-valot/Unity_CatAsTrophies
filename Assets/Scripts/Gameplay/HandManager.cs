using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;
    
    [Header("REFERENCES")]
    public Transform[] handPoints;
    
    [Header("DEBUGGING")]
    public string[] catsInHand;

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        Instance.catsInHand = new[] {"", "", "", "", ""};
    }

    /// <summary>
    /// Spawn a cat and add it to the player's hand
    /// </summary>
    /// <param name="_catIndex">Type index of the cat</param>
    public void DrawCat(int _catIndex)
    {
        Cat catSpawned = CatGenerator.Instance.SpawnCatGraphics(_catIndex, GetAvailablePosition());
        catSpawned.state = CatState.InHand;
        
        Instance.AddToHand(catSpawned.id);
    }
    
    /// <summary>
    /// Add a cat to the player's hand
    /// </summary>
    public void AddToHand(string _catId)
    {
        for (int i = 0; i < Instance.catsInHand.Length; i++)
        {
            if (Instance.catsInHand[i] == "")
            {
                Instance.catsInHand[i] = _catId;
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
            catsInHand[i] = catsInHand[i] == _catId ? "" : catsInHand[i];
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
                Misc.GetCatById(CatGenerator.Instance.cats, catsInHand[i]).Withdraw();
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
        
        for (int i = 0; i < Instance.catsInHand.Length; i++)
        {
            if (Instance.catsInHand[i] == "")
            {
                output = Instance.handPoints[i].position;
                break;
            }
        }

        if (output == Vector3.one)
        {
            Debug.LogError("HAND MANAGER: cats in hand limit reach. can't add move cat.", this);
            Debug.Break();
        }
        
        return output;
    }
}