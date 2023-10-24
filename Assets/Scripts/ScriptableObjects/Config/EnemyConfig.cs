using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Config/Enemy", order = 1)]
public class EnemyConfig : ScriptableObject
{
    [Header("STATS")]
    public string catName;
    public float strength;
    public float health;

    [Header("GRAPHICS")]
    public GameObject catBasePrefab;
    public GameObject rightHandAddon;
    public GameObject leftHandAddon;
    public GameObject headAddon;
}