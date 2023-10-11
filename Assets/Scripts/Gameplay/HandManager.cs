using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;
    
    [Header("REFERENCES")]
    public Transform[] handPoints;
    
    [Header("DEBUGGING")]
    public int[] catsInHand;

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        Instance.catsInHand = new[] { -1, -1, -1, -1, -1 };
    }

    public void AddCat(int _catIndex)
    {
        for (int i = 0; i < Instance.catsInHand.Length; i++)
        {
            if (Instance.catsInHand[i] == -1)
            {
                CatGenerator.Instance.SpawnCat(_catIndex, GetAvailablePosition());
                Instance.catsInHand[i] = _catIndex;
                break;
            }
        }
    }

    public void RemoveCat(int _catToRemove)
    {
        for (int i = 0; i < Instance.catsInHand.Length; i++)
        {
            if (Instance.catsInHand[i] == _catToRemove)
            {
                Instance.catsInHand[i] = -1;
            }
        }
    }

    public Vector3 GetAvailablePosition()
    {
        Vector3 output = Vector3.one;
        
        for (int i = 0; i < Instance.catsInHand.Length; i++)
        {
            if (Instance.catsInHand[i] == -1)
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