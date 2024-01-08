using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;
    
    public List<Entity> entities;

    private void Awake() => Instance = this;
}