using UnityEngine;

[CreateAssetMenu(fileName = "Cat", menuName = "Config/Cat", order = 1)]
public class CatConfig : ScriptableObject
{
    [Header("STATS")]
    public string catName;
    public float strengh;
    public float health;

    [Header("GRAPHICS")]
    public GameObject catBasePrefab;
    public GameObject rightHandAddon;
    public GameObject leftHandAddon;
    public GameObject headAddon;
}