using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PlayerDeckWindowEditor : EditorWindow
    {
        #region INITIALIZATION
        private PlayerConfig _playerConfig;

        private List<EntityConfig> _catsConfig = new List<EntityConfig>();
        private List<int> _catsCount = new List<int>();
    
        private readonly List<String> _catsName = new List<string>();
        private readonly List<int> _catsIndex = new List<int>();  
    
        // window editor component
        private Vector2 _informationsScroll;
        private EntitiesConfig _entitiesConfig;
        #endregion
    
        [MenuItem("Integration Tools/Player deck")]
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
            _catsIndex.Clear();
            for (int i = 0; i < 3; i++)
            {
                _catsIndex.Add(0);
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
                _informationsScroll = EditorGUILayout.BeginScrollView(_informationsScroll);
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
            _catsIndex.Clear();
            _catsName.Clear();
        
            foreach (var cat in _playerConfig.deckEntities)
            {
                _catsIndex.Add(_entitiesConfig.cats.IndexOf(cat));
            }
        }
    
        private int GetTotalNumberOfCats()
        {
            int output = 0;
            foreach (int catCount in _catsCount)
            {
                output += catCount;
            }
            return output;
        }
    
        private void DisplayCatsChoser()
        {
            // exit if 
            if (_entitiesConfig.cats.Count <= 0)
            {
                Debug.LogError("PLAYER DECK WINDOW EDITOR: there is no cats in entities config", this);
                return;
            }
        
            GUILayout.BeginVertical("HelpBox");
            {
                // get all cats name in a list of string
                foreach (EntityConfig cat in _entitiesConfig.cats)
                {
                    _catsName.Add(cat.entityName);
                    _catsIndex.Add(0);
                }
            
                // HEADER
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Cats");
                    if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                    {
                        _catsConfig.Add(_entitiesConfig.cats[0]);
                        _catsCount.Add(1);
                    }
                }
                GUILayout.EndHorizontal();

                // LIST
                if (_catsConfig != null)
                {
                    for (int index = 0; index < _catsConfig.Count; index++)
                    {
                        DisplayCatPlacement(index);
                    }
                }
            }
            GUILayout.EndVertical();
        }

        private void DisplayCatPlacement(int index)
        {
            GUILayout.BeginHorizontal("HelpBox");
            {
                _catsIndex[index] = EditorGUILayout.Popup("", _catsIndex[index], _catsName.ToArray());
                _catsConfig[index] = _entitiesConfig.cats[_catsIndex[index]];
                        
                _catsCount[index] = EditorGUILayout.IntField("Count", _catsCount[index]);
                        
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    if (EditorUtility.DisplayDialog("Delete entity", "Do you really want to permanently delete this entity?", "Yes", "No"))
                    {
                        _catsConfig.Remove(_catsConfig[index]);
                        _catsCount.Remove(_catsCount[index]);
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
            _playerConfig.deckEntities = _catsConfig;
            _playerConfig.deckEntitiesCount = _catsCount;
        
            // save changes
            EditorUtility.SetDirty(_playerConfig);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        
            // update entities config
            UpdateEntitiesConfig();
        }

        private void LoadDataFromAsset()
        {
            _entitiesConfig = EditorMisc.FindEntitiesConfig();
            _playerConfig = EditorMisc.FindPlayerConfig();
            _catsConfig = _playerConfig.deckEntities;
            _catsCount = _playerConfig.deckEntitiesCount;
        }
    
        private void UpdateEntitiesConfig()
        {
            _playerConfig.deckEntities = _catsConfig;
            _playerConfig.deckEntitiesCount = _catsCount;
            _playerConfig.deckLenght = GetTotalNumberOfCats();
            EditorUtility.SetDirty(_playerConfig);
        }
    }
}