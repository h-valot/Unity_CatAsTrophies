using UnityEngine;

[CreateAssetMenu(fileName = "CatsConfig", menuName = "Config/Cats", order = 1)]
public class CatsConfig : ScriptableObject
{
    public CatConfig[] cats;
}