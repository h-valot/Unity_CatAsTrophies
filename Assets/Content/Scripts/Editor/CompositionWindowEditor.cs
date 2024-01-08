using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CompositionWindowEditor : EditorWindow
    {
        #region INITIALIZATION
        // lists
        private readonly List<CompositionConfig> _compositions = new List<CompositionConfig>();

        private CompositionConfig _currentComposition;
        private string _compositionName;
        private CompositionTier _compositionTier;
        private List<EntityConfig> _enemies = new List<EntityConfig>();
        
        private readonly List<String> _enemiesName = new List<string>();
        private readonly List<int> _enemiesIndex = new List<int>();
    
        // window editor component
        private int _id;
        private Vector2 _sideBarScroll, _informationsScroll;
        private bool _canDisplayDetails = true;
        private EntitiesConfig _entitiesConfig;
        #endregion
    
        [MenuItem("Integration Tools/Compositions")]
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
            _compositions.Clear();
            _enemiesIndex.Clear();
            for (int i = 0; i < 3; i++)
            {
                _enemiesIndex.Add(0);
            }
        
            // load the data from the entities config
            LoadDataFromAsset();
        
            // display the first composition in the list if isn't empty
            if (_compositions.Count > 0)
            {
                UpdateInformations(_compositions[0]);
            }
            else
            {
                _canDisplayDetails = false;
            }
        }
    
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical("HelpBox", GUILayout.Width(175), GUILayout.ExpandHeight(true));
                {
                    _sideBarScroll = EditorGUILayout.BeginScrollView(_sideBarScroll);
                    {
                        DisplaySideBar();
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            
                EditorGUILayout.BeginVertical();
                {
                    if (_canDisplayDetails)
                    {
                        _informationsScroll = EditorGUILayout.BeginScrollView(_informationsScroll);
                        {
                            DisplayInformations();
                        }
                        EditorGUILayout.EndScrollView();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
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
        
                    _compositions.Add(newInstance);
                    UpdateInformations(newInstance);
                    _canDisplayDetails = true;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            // LIST OF CATS
            foreach (CompositionConfig composition in _compositions)
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
            _compositionName = EditorGUILayout.TextField("Name", _compositionName);
            _compositionTier = (CompositionTier)EditorGUILayout.EnumPopup("Tier", _compositionTier);
        
        
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
    
        private void UpdateInformations(CompositionConfig compositionConfig)
        {
            _currentComposition = compositionConfig;
        
            // BASE INFORMATIONS
            _enemies = _currentComposition.entities;
            _compositionName = _currentComposition.compositionName;
            _compositionTier = _currentComposition.tier;

            // INDEX TRANSPOSITIONS
            for (int i = 0; i < 3; i++)
            {
                _enemiesIndex[i] = _entitiesConfig.enemies.IndexOf(_currentComposition.entities[i]) + 1; 
            }
        
            _canDisplayDetails = true;
        }

        private void DisplayEnemiesChoser()
        {
            GUILayout.BeginVertical("HelpBox");
            {
                _enemiesName.Add("None");
                foreach (EntityConfig enemy in _entitiesConfig.enemies)
                {
                    _enemiesName.Add(enemy.entityName);
                }

                DisplayEnemyPlacement("Front", 0);
                DisplayEnemyPlacement("Middle", 1);
                DisplayEnemyPlacement("Back", 2);
            }
            GUILayout.EndVertical();
        }

        private void DisplayEnemyPlacement(string label, int index)
        {
            GUILayout.BeginHorizontal();
            {
                _enemiesIndex[index] = EditorGUILayout.Popup(label, _enemiesIndex[index], _enemiesName.ToArray());

                if (_enemiesIndex[index] == 0)
                {
                    _enemies[index] = null;
                }
                else
                {
                    _enemies[index] = _entitiesConfig.enemies[_enemiesIndex[index] - 1];
                }
            
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    _enemiesIndex[index] = 0;
                    _enemies[index] = null;
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
        }
    
        private void SaveDataToAsset()
        {
            // exceptions
            if (_compositionName == "") return;
            
            // update data into current entity
            _currentComposition.entities = _enemies;
            _currentComposition.compositionName = _compositionName;
            _currentComposition.tier = _compositionTier;

            // get the path
            string path = $"Assets/Configs/Compositions/{_currentComposition.id}.asset";

            // if the asset does not already exists then create a new one 
            if (!AssetDatabase.LoadAssetAtPath<CompositionConfig>(path))
            {
                AssetDatabase.CreateAsset(_currentComposition, path);
            }
        
            // save changes
            EditorUtility.SetDirty(_currentComposition);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        
            // update entities config
            UpdateEntitiesConfig();
        }

        private void DeleteAssetData()
        {
            // remove from lists
            _compositions.Remove(_currentComposition);

            // deleting the asset
            AssetDatabase.DeleteAsset($"Assets/Configs/Compositions/{_currentComposition.id}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        
            // update entities config
            UpdateEntitiesConfig();
        }
    
        private void LoadDataFromAsset()
        {
            // load entities config
            _entitiesConfig = EditorMisc.FindEntitiesConfig();
        
            // get all files with type "CompositionConfig" in the project
            string[] fileGuidsArray = AssetDatabase.FindAssets("t:" + typeof(CompositionConfig));

            // fulfill cats and enemies with CompositionConfig 
            foreach (string fileGuid in fileGuidsArray)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(fileGuid);
                CompositionConfig compositionConfig = AssetDatabase.LoadAssetAtPath<CompositionConfig>(assetPath);
                if (!compositionConfig.isPlayerDeck)
                {
                    _compositions.Add(compositionConfig);
                }
            }
        }

        private void UpdateEntitiesConfig()
        {
            _entitiesConfig = EditorMisc.FindEntitiesConfig();
            _entitiesConfig.compositions = _compositions;
            EditorUtility.SetDirty(_entitiesConfig);
        }
    }
}