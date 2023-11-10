using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CompositionWindowEditor : EditorWindow
{
    #region INITIALIZATION
    // lists
    private List<EntityConfig> enemies = new List<EntityConfig>();
    private List<CompositionConfig> compositions = new List<CompositionConfig>();

    private CompositionConfig currentComposition;
    private string compositionName;
    private List<String> enemiesName = new List<string>();
    private List<int> enemiesIndex = new List<int>();
        
    
    // window editor component
    private int id;
    private Vector2 sideBarScroll, informationsScroll;
    private bool canDisplayDetails = true;
    #endregion
    
    [MenuItem("Tool/Compositions")]
    static void InitializeWindow()
    {
        CompositionWindowEditor window = GetWindow<CompositionWindowEditor>();
        window.titleContent = new GUIContent("Composition Integration");
        window.maxSize = new Vector2(801, 421);
        window.minSize = new Vector2(800, 420);
        window.Show();
    }
    
    private void OnEnable()
    {
        // initialize lists
        compositions.Clear();
        enemiesIndex.Clear();
        for (int i = 0; i < 3; i++)
        {
            enemiesIndex.Add(0);
        }
        
        // load the data from the entities config
        LoadDataFromAsset();
        
        // display the first composition in the list if isn't empty
        if (compositions.Count > 0)
        {
            UpdateInformations(compositions[0]);
        }
        else
        {
            canDisplayDetails = false;
        }
    }
    
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical("HelpBox", GUILayout.Width(175), GUILayout.ExpandHeight(true));
            {
                sideBarScroll = EditorGUILayout.BeginScrollView(sideBarScroll);
                {
                    DisplaySideBar();
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            {
                if (canDisplayDetails)
                {
                    informationsScroll = EditorGUILayout.BeginScrollView(informationsScroll);
                    {
                        DisplayInformations();
                    }
                    EditorGUILayout.EndScrollView();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
        
        // update modifications
        UpdateEntitiesConfig();
    }
    
    private void DisplaySideBar()
    {
        // HEADER
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("COMPOSITIONS", EditorStyles.boldLabel);
            if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
            {
                var newInstance = CreateInstance<CompositionConfig>();
                newInstance.Initialize();
                var date = DateTime.Now;
                newInstance.id = "Comp_" + date.ToString("yyyyMMdd_HHmmss_fff");
        
                compositions.Add(newInstance);
                UpdateInformations(newInstance);
                canDisplayDetails = true;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        // LIST OF CATS
        foreach (CompositionConfig composition in compositions)
        {
            string buttonName = composition.compositionName == "" ? "New composition" : composition.compositionName;
            if (GUILayout.Button($"{buttonName}", GUILayout.ExpandWidth(true), GUILayout.Height(20)))
            {
                UpdateInformations(composition);
            }
        }
    }
    
    private void DisplayInformations()
    {
        // INFORMATIONS
        GUILayout.Label("INFORMATION", EditorStyles.boldLabel);
        compositionName = EditorGUILayout.TextField("Name", compositionName);
        
        
        // COMPOSITIONS
        GUILayout.Space(10);
        GUILayout.Label("COMPOSITION", EditorStyles.boldLabel);
        DisplayEnemiesChoser();
        
        
        // DATA MANAGEMENT
        // save button
        GUILayout.Space(20);
        if (GUILayout.Button("SAVE", GUILayout.ExpandWidth(true)))
        {
            SaveDataToAsset();
        }
    
        // delete button
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("DELETE", GUILayout.ExpandWidth(true)))
        {
            if (EditorUtility.DisplayDialog("Delete Entity", "Do you really want to permanently delete this entity?", "Yes", "No"))
            {
                DeleteAssetData();
            }
        }
        GUI.backgroundColor = Color.white;
    }
    
    private void UpdateInformations(CompositionConfig _compositionConfig)
    {
        currentComposition = _compositionConfig;
        
        // BASE INFORMATIONS
        compositionName = currentComposition.compositionName;
        enemies = currentComposition.entities;

        // INDEX TRANSPOSITIONS
        for (int i = 0; i < 3; i++)
        {
            enemiesIndex[i] = EditorMisc.FindEntitiesConfig().enemies.IndexOf(currentComposition.entities[i]) + 1; 
        }
        
        canDisplayDetails = true;
    }

    private void DisplayEnemiesChoser()
    {
        GUILayout.BeginVertical("HelpBox");
        {
            enemiesName.Add("None");
            foreach (EntityConfig enemy in EditorMisc.FindEntitiesConfig().enemies)
            {
                enemiesName.Add(enemy.entityName);
            }

            DisplayEnemyPlacement("Front", 0);
            DisplayEnemyPlacement("Middle", 1);
            DisplayEnemyPlacement("Back", 2);
        }
        GUILayout.EndVertical();
    }

    private void DisplayEnemyPlacement(string _label, int _index)
    {
        GUILayout.BeginHorizontal();
        {
            enemiesIndex[_index] = EditorGUILayout.Popup(_label, enemiesIndex[_index], enemiesName.ToArray());

            if (enemiesIndex[_index] == 0)
            {
                enemies[_index] = null;
            }
            else
            {
                enemies[_index] = EditorMisc.FindEntitiesConfig().enemies[enemiesIndex[_index] - 1];
            }
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
            {
                enemiesIndex[_index] = 0;
                enemies[_index] = null;
            }
            GUI.backgroundColor = Color.white;
        }
        GUILayout.EndHorizontal();
    }
    
    private void SaveDataToAsset()
    {
        // exceptions
        if (compositionName == "") return;
            
        // update data into current entity
        currentComposition.entities = enemies;
        currentComposition.compositionName = compositionName;

        // get the path
        string path = $"Assets/Configs/Compositions/{currentComposition.id}.asset";

        // if the asset does not already exists then create a new one 
        if (!AssetDatabase.LoadAssetAtPath<CompositionConfig>(path))
        {
            AssetDatabase.CreateAsset(currentComposition, path);
        }
        
        // save changes
        EditorUtility.SetDirty(currentComposition);
        EditorUtility.SetDirty(EditorMisc.FindEntitiesConfig());
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void DeleteAssetData()
    {
        // remove from lists
        compositions.Remove(currentComposition);

        // deleting the asset
        AssetDatabase.DeleteAsset($"Assets/Configs/Compositions/{currentComposition.id}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private void LoadDataFromAsset()
    {
        // get all files with type "CompositionConfig" in the project
        string[] fileGuidsArray = AssetDatabase.FindAssets("t:" + typeof(CompositionConfig));

        // fulfill cats and enemies with CompositionConfig 
        foreach (string fileGuid in fileGuidsArray)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuid);
            CompositionConfig compositionConfig = AssetDatabase.LoadAssetAtPath<CompositionConfig>(assetPath);
            if (!compositionConfig.isPlayerDeck)
            {
                compositions.Add(compositionConfig);
            }
        }
    }
    
    private void UpdateEntitiesConfig()
    {
        EditorMisc.FindEntitiesConfig().compositions = compositions;
    }
}