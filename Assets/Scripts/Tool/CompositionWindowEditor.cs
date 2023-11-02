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

    private List<String> stringList = new List<string>();
    private int frontEnemyIndex, middleEnemyIndex, backEnemyIndex;
    
    // window editor component
    private int id;
    private Vector2 sideScrollPos, detailsScrollPos;
    private bool canDisplayDetails = true;
    #endregion
    
    [MenuItem("Tool/Composition Integration")]
    static void InitializeWindow()
    {
        CompositionWindowEditor window = GetWindow<CompositionWindowEditor>();
        window.titleContent = new GUIContent("Entity Integration");
        window.Show();
    }
    
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical("HelpBox", GUILayout.Width(175), GUILayout.ExpandHeight(true));
            {
                DisplaySideLists();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            {
                if (canDisplayDetails)
                {
                    DisplayDetails();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnEnable()
    {
        compositions.Clear();
        
        // load the data from the entities config
        LoadData();
        
        if (enemies.Count > 0)
        {
            UpdateDetails(compositions[0]);
        }
        else
        {
            canDisplayDetails = false;
        }
    }
    
    private void OnDisable()
    {
        UpdateEntitiesConfig();
    }
    
    private void DisplaySideLists()
    {
        sideScrollPos = EditorGUILayout.BeginScrollView(sideScrollPos);
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
                    UpdateDetails(newInstance);
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
                    UpdateDetails(composition);
                }
            }
        }
        EditorGUILayout.EndScrollView();
    }
    
    private void DisplayDetails()
    {
        detailsScrollPos = EditorGUILayout.BeginScrollView(detailsScrollPos);
        {
            // INFORMATIONS
            GUILayout.Label("INFORMATION", EditorStyles.boldLabel);
            compositionName = EditorGUILayout.TextField("Name", compositionName);
            
            
            // COMPOSITIONS
            GUILayout.Space(10);
            GUILayout.Label("COMPOSITION", EditorStyles.boldLabel);

            GUILayout.BeginVertical("HelpBox");
            {
                foreach (var entity in FindEntitiesConfig().enemies)
                {
                    stringList.Add(entity.entityName);
                }

                GUILayout.BeginVertical();
                {
                    frontEnemyIndex = EditorGUILayout.Popup($"Front enemy", frontEnemyIndex, stringList.ToArray());
                    enemies[0] = FindEntitiesConfig().enemies[frontEnemyIndex];
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                {
                    middleEnemyIndex = EditorGUILayout.Popup($"Middle enemy", middleEnemyIndex, stringList.ToArray());
                    enemies[1] = FindEntitiesConfig().enemies[middleEnemyIndex];
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                {
                    backEnemyIndex = EditorGUILayout.Popup($"Back enemy", backEnemyIndex, stringList.ToArray());
                    enemies[2] = FindEntitiesConfig().enemies[backEnemyIndex];
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
            
            
            // DATA MANAGEMENT
            // save button
            GUILayout.Space(20);
            if (GUILayout.Button("SAVE", GUILayout.ExpandWidth(true)))
            {
                SaveData();
            }
        
            // delete button
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("DELETE", GUILayout.ExpandWidth(true)))
            {
                if (EditorUtility.DisplayDialog("Delete Entity", "Do you really want to permanently delete this entity?", "Yes", "No"))
                {
                    DeleteData();
                }
            }
            GUI.backgroundColor = Color.white;
        }
        EditorGUILayout.EndScrollView();
    }
    
    private void SaveData()
    {
        // exceptions
        if (compositionName == "") return;
            
        // update data into current entity
        currentComposition.enemies = enemies;
        currentComposition.compositionName = compositionName;

        // get the path
        string path = $"Assets/Configs/Compositions/{currentComposition.id}.asset";

        // if the asset does not already exists then create a new one 
        if (!AssetDatabase.LoadAssetAtPath<CompositionConfig>(path))
        {
            AssetDatabase.CreateAsset(currentComposition, path);
        }
        
        // save changes
        AssetDatabase.SaveAssets();
    }

    private void DeleteData()
    {
        // remove from lists
        compositions.Remove(currentComposition);

        // deleting the asset
        AssetDatabase.DeleteAsset($"Assets/Configs/Compositions/{currentComposition.id}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private void LoadData()
    {
        // get all files with type "CompositionConfig" in the project
        string[] fileGuidsArray = AssetDatabase.FindAssets("t:" + typeof(CompositionConfig));

        // fulfill cats and enemies with CompositionConfig 
        foreach (string fileGuid in fileGuidsArray)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuid);
            CompositionConfig compositionConfig = AssetDatabase.LoadAssetAtPath<CompositionConfig>(assetPath);
            compositions.Add(compositionConfig);
        }
    }
    
    private void UpdateDetails(CompositionConfig _compositionConfig)
    {
        currentComposition = _compositionConfig;
        
        compositionName = currentComposition.compositionName;
        enemies = currentComposition.enemies;

        canDisplayDetails = true;
    }
    
    private static EntitiesConfig FindEntitiesConfig()
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
            EntitiesConfig entitiesConfig = CreateInstance<EntitiesConfig>();
            AssetDatabase.CreateAsset(entitiesConfig, "Assets/Configs/EntitiesConfig.asset");
            AssetDatabase.SaveAssets();
            return entitiesConfig;
        }
    }
    
    /// <summary>
    /// Update cats and enemies list in the EntitiesConfig
    /// </summary>
    private void UpdateEntitiesConfig()
    {
        FindEntitiesConfig().compositions.Clear();
        FindEntitiesConfig().compositions = compositions;
    }
}