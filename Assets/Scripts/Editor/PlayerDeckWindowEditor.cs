using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerDeckWindowEditor : EditorWindow
{
    #region INITIALIZATION
    private PlayerConfig playerConfig;

    private List<EntityConfig> catsConfig = new List<EntityConfig>();
    private List<int> catsCount = new List<int>();
    private List<String> catsName = new List<string>();
    private List<int> catsIndex = new List<int>();  
    
    // window editor component
    private Vector2 informationsScroll;
    private EntitiesConfig entitiesConfig;
    #endregion
    
    [MenuItem("Tool/Player deck")]
    static void InitializeWindow()
    {
        PlayerDeckWindowEditor window = GetWindow<PlayerDeckWindowEditor>();
        window.titleContent = new GUIContent("Player deck editor");
        window.maxSize = new Vector2(801, 421);
        window.minSize = new Vector2(800, 420);
        window.Show();
    }
    
    private void OnEnable()
    {
        // initialize lists
        catsIndex.Clear();
        for (int i = 0; i < 3; i++)
        {
            catsIndex.Add(0);
        }

        // load data
        LoadDataFromAsset();
        
        // update information displayer
        UpdateInformations();
    }
    
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            informationsScroll = EditorGUILayout.BeginScrollView(informationsScroll);
            {
                DisplayInformations();
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndHorizontal();
    }
    
    private void DisplayInformations()
    {
        // INFORMATIONS
        GUILayout.Label("PLAYER'S DECK", EditorStyles.boldLabel);
        DisplayCatsChoser();
        GUILayout.Label($"There is a total of {GetTotalNumberOfCats()} cats in the player's deck.");
        
        // DATA MANAGEMENT
        // save button
        GUILayout.Space(20);
        if (GUILayout.Button("SAVE", GUILayout.ExpandWidth(true)))
        {
            SaveDataToAsset();
        }
    }

    private void UpdateInformations()
    {
        catsIndex.Clear();
        catsName.Clear();
        
        foreach (var cat in playerConfig.deckEntities)
        {
            catsIndex.Add(entitiesConfig.cats.IndexOf(cat));
        }
    }
    
    private int GetTotalNumberOfCats()
    {
        int output = 0;
        foreach (int catCount in catsCount)
        {
            output += catCount;
        }
        return output;
    }
    
    private void DisplayCatsChoser()
    {
        // exit if 
        if (entitiesConfig.cats.Count <= 0)
        {
            Debug.LogError("PLAYER DECK WINDOW EDITOR: there is no cats in entities config", this);
            return;
        }
        
        GUILayout.BeginVertical("HelpBox");
        {
            // get all cats name in a list of string
            foreach (EntityConfig cat in entitiesConfig.cats)
            {
                catsName.Add(cat.entityName);
                catsIndex.Add(0);
            }
            
            // HEADER
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Cats");
                if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    catsConfig.Add(entitiesConfig.cats[0]);
                    catsCount.Add(1);
                }
            }
            GUILayout.EndHorizontal();

            // LIST
            if (catsConfig != null)
            {
                for (int index = 0; index < catsConfig.Count; index++)
                {
                    DisplayCatPlacement(index);
                }
            }
        }
        GUILayout.EndVertical();
    }

    private void DisplayCatPlacement(int _index)
    {
        GUILayout.BeginHorizontal("HelpBox");
        {
            catsIndex[_index] = EditorGUILayout.Popup("", catsIndex[_index], catsName.ToArray());
            catsConfig[_index] = entitiesConfig.cats[catsIndex[_index]];
                        
            catsCount[_index] = EditorGUILayout.IntField("Count", catsCount[_index]);
                        
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
            {
                if (EditorUtility.DisplayDialog("Delete entity", "Do you really want to permanently delete this entity?", "Yes", "No"))
                {
                    catsConfig.Remove(catsConfig[_index]);
                    catsCount.Remove(catsCount[_index]);
                    return;
                }
            }
            GUI.backgroundColor = Color.white;
        }
        GUILayout.EndHorizontal();
    }
    
    private void SaveDataToAsset()
    {
        // save data into a temp game asset
        playerConfig.deckEntities = catsConfig;
        playerConfig.deckEntitiesCount = catsCount;
        
        // save changes
        EditorUtility.SetDirty(playerConfig);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        // update entities config
        UpdateEntitiesConfig();
    }

    private void LoadDataFromAsset()
    {
        entitiesConfig = EditorMisc.FindEntitiesConfig();
        playerConfig = EditorMisc.FindPlayerConfig();
        catsConfig = playerConfig.deckEntities;
        catsCount = playerConfig.deckEntitiesCount;
    }
    
    private void UpdateEntitiesConfig()
    {
        playerConfig.deckEntities = catsConfig;
        playerConfig.deckEntitiesCount = catsCount;
        playerConfig.deckLenght = GetTotalNumberOfCats();
        EditorUtility.SetDirty(playerConfig);
    }
}