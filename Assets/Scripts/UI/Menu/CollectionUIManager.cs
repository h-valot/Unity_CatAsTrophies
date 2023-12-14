using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CollectionUIManager : MonoBehaviour
{
    [SerializeField] private GameObject Cat;
    [SerializeField] private Transform panel; 

    public void AddCat()
    {
        Instantiate(Cat,panel);
    }
}
