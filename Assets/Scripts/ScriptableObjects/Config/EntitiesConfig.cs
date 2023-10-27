using UnityEngine;

[CreateAssetMenu(fileName = "EntitiesConfig", menuName = "Config/Entities", order = 1)]
public class EntitiesConfig : ScriptableObject
{
    public EntityConfig[] cats;
    public EntityConfig[] enemies;
}