using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Config/GameSettings", order = 2)]
public class GameSettings : ScriptableObject
{
    public string startingScene;
}