using UnityEditor;
using UnityEngine;

public static class EditorMisc
{
    public static EntitiesConfig FindEntitiesConfig()
    {
        // get all files with type "EntitiesConfig" in the project
        string[] fileGuidsArray = AssetDatabase.FindAssets("t:" + typeof(EntitiesConfig));
        
        if (fileGuidsArray.Length > 0)
        {
            // if file exists, get first EntitiesConfig and return it
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuidsArray[0]);
            return AssetDatabase.LoadAssetAtPath<EntitiesConfig>(assetPath);
        }
        else
        {
            // if file does not exist, create a new EntitiesConfig and save it into a dedicated path
            EntitiesConfig entitiesConfig = ScriptableObject.CreateInstance<EntitiesConfig>();
            AssetDatabase.CreateAsset(entitiesConfig, "Assets/Configs/EntitiesConfig.asset");
            AssetDatabase.SaveAssets();
            return entitiesConfig;
        }
    }
    
    public static PlayerConfig FindPlayerConfig()
    {
        // get all files with type "PlayerConfig" in the project
        string[] fileGuidsArray = AssetDatabase.FindAssets("t:" + typeof(PlayerConfig));
        
        if (fileGuidsArray.Length > 0)
        {
            // if file exists, get first PlayerConfig and return it
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuidsArray[0]);
            return AssetDatabase.LoadAssetAtPath<PlayerConfig>(assetPath);
        }
        else
        {
            // if file does not exist, create a new PlayerConfig and save it into a dedicated path
            PlayerConfig playerConfig = ScriptableObject.CreateInstance<PlayerConfig>();
            AssetDatabase.CreateAsset(playerConfig, "Assets/Configs/PlayerConfig.asset");
            AssetDatabase.SaveAssets();
            return playerConfig;
        }
    }
}