using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropPlane : MonoBehaviour
{
    public static DragAndDropPlane Instance;

    public MeshCollider meshCollider;

    private void Awake()
    {
        Instance = this;
        meshCollider.enabled = false;
    }
}
