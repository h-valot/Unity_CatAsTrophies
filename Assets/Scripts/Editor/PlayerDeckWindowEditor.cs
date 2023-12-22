using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Data.Player;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PlayerDeckWindowEditor : EditorWindow
    {
        #region INITIALIZATION
        private PlayerConfig _playerConfig;

        private List<Item> _deck = new List<Item>();
    
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
            UpdateInformation();
        }
    
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                _informationsScroll = EditorGUILayout.BeginScrollView(_informationsScroll);
                {
                    DisplayInformation();
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndHorizontal();
        }
    
        private void DisplayInformation()
        {
            // INFORMATION
            GUILayout.Label("PLAYER'S DECK", EditorStyles.boldLabel);
            DisplayCatsChooser();
            GUILayout.Label($"There is a total of {GetDeckLenght()} cats in the player's deck.");
        
            // DATA MANAGEMENT
            // save button
            GUILayout.Space(20);
            if (GUILayout.Button("SAVE", GUILayout.ExpandWidth(true)))
            {
                SaveDataToAsset();
            }
        }

        private void UpdateInformation()
        {
            _catsIndex.Clear();
            _catsName.Clear();
        
            foreach (var item in _playerConfig.deck)
            {
                _catsIndex.Add(item.entityIndex);
            }
        }
    
        private void DisplayCatsChooser()
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
                        _deck.Add(new Item());
                    }
                }
                GUILayout.EndHorizontal();

                // LIST
                if (_deck != null)
                {
                    for (int index = 0; index < _deck.Count; index++)
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
                _deck[index].entityIndex = _catsIndex[index];
                
                if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    _deck[index].Remove();
                }
                GUILayout.Label($"{_deck[index].cats.Count}", GUILayout.Width(20), GUILayout.Height(20));
                if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    _deck[index].Add(new CatData(_deck[index].entityIndex, _entitiesConfig.cats[_deck[index].entityIndex].health));
                }
                
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    if (EditorUtility.DisplayDialog("Delete entity", "Do you really want to permanently delete this entity?", "Yes", "No"))
                    {
                        _deck.Remove(_deck[index]);
                        return;
                    }
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
        }

        private int GetDeckLenght()
        {
            var output = 0;
            foreach (var item in _deck)
            {
                output += item.cats.Count;
            }
            return output;
        }
        
        private void SaveDataToAsset()
        {
            // save data into a temp game asset
            _playerConfig.deck = _deck;
        
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
            _deck = _playerConfig.deck;
        }
    
        private void UpdateEntitiesConfig()
        {
            _playerConfig.deck = _deck;
            _playerConfig.deckLenght = GetDeckLenght();
            EditorUtility.SetDirty(_playerConfig);
        }
    }
}