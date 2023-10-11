using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;
    
    [Header("REFERENCES")]
    [SerializeField] private CatGenerator catGenerator;
    public Transform[] handPoints;
    
    [Header("DEBUGGING")]
    public int[] catsInHand;

    private void Awake() => Instance = this;
    
    private void Start()
    {
        catsInHand = new[] { -1, -1, -1, -1, -1 };
    }

    public void AddCat(int newCatIndex)
    {
        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] == -1)
            {
                catGenerator.CreateCatGraphics(newCatIndex, GetNewCatPositionInHand());
                catsInHand[i] = newCatIndex;
                break;
            }
        }
    }

    public void RemoveCat(int catToRemove)
    {
        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] == catToRemove)
            {
                catsInHand[i] = -1;
            }
        }
    }

    public Vector3 GetNewCatPositionInHand()
    {
        Vector3 output = Vector3.one;
        
        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] == -1)
            {
                output = handPoints[i].position;
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