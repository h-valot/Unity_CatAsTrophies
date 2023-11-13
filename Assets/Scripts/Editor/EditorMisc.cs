using UnityEditor;
using UnityEngine;

public static class EditorMisc
{
    private static EntitiesConfig entitiesConfigCache;
    private static PlayerConfig playerConfigCache;
    
    public static EntitiesConfig FindEntitiesConfig()
    {
        // return the load already loaded in cache asset
        if (entitiesConfigCache) return entitiesConfigCache;
        
        // else find this asset
        // get all files with type "EntitiesConfig" in the project
        string[] fileGuidsArray = AssetDatabase.FindAssets("t:" + typeof(EntitiesConfig));
        
        if (fileGuidsArray.Length > 0)
        {
            // if file exists, get first EntitiesConfig and return it
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuidsArray[0]);
            entitiesConfigCache = AssetDatabase.LoadAssetAtPath<EntitiesConfig>(assetPath);
        }
        else
        {
            // if file does not exist, create a new EntitiesConfig and save it into a dedicated path
            EntitiesConfig entitiesConfig = ScriptableObject.CreateInstance<EntitiesConfig>();
            AssetDatabase.CreateAsset(entitiesConfig, "Assets/Configs/EntitiesConfig.asset");
            AssetDatabase.SaveAssets();
            entitiesConfigCache = entitiesConfig;
        }

        return entitiesConfigCache;
    }
    
    public static PlayerConfig FindPlayerConfig()
    {
        // return the load already loaded in cache asset
        if (playerConfigCache) return playerConfigCache;
        
        // else find this asset
        // get all files with type "PlayerConfig" in the project
        string[] fileGuidsArray = AssetDatabase.FindAssets("t:" + typeof(PlayerConfig));
        
        if (fileGuidsArray.Length > 0)
        {
            // if file exists, get first PlayerConfig and return it
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuidsArray[0]);
            playerConfigCache = AssetDatabase.LoadAssetAtPath<PlayerConfig>(assetPath);
        }
        else
        {
            // if file does not exist, create a new PlayerConfig and save it into a dedicated path
            PlayerConfig playerConfig = ScriptableObject.CreateInstance<PlayerConfig>();
            AssetDatabase.CreateAsset(playerConfig, "Assets/Configs/PlayerConfig.asset");
            AssetDatabase.SaveAssets();
            playerConfigCache = playerConfig;
        }

        return playerConfigCache;
    }
}