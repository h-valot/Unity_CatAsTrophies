using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PlayerDeckWindowEditor : EditorWindow
    {
        #region INITIALIZATION
        private PlayerConfig _playerConfig;

        private List<EditorItem> _editorItems = new List<EditorItem>();
        private List<Item> _items = new List<Item>();
    
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
            GUILayout.Label($"There is a total of {ConvertToItems(_editorItems).Count} cats in the player's deck.");
        
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
        
            foreach (var editorItem in ConvertToEditorItems(_playerConfig.deck))
            {
                _catsIndex.Add(editorItem.entityIndex);
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
                        _editorItems.Add(new EditorItem(_entitiesConfig.cats[0].health));
                    }
                }
                GUILayout.EndHorizontal();

                // LIST
                if (_editorItems != null)
                {
                    for (int index = 0; index < _editorItems.Count; index++)
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
                _editorItems[index].entityIndex = _catsIndex[index];
                
                if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    _editorItems[index].amount--;
                    if (_editorItems[index].amount == 0) _editorItems.Remove(_editorItems[index]);
                }
                GUILayout.Label($"{_editorItems[index].amount}", GUILayout.Width(20), GUILayout.Height(20));
                if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    _editorItems[index].amount++;
                }
                
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    if (EditorUtility.DisplayDialog("Delete entity", "Do you really want to permanently delete this entity?", "Yes", "No"))
                    {
                        _editorItems.Remove(_editorItems[index]);
                        return;
                    }
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
        }

        private List<Item> ConvertToItems(List<EditorItem> editorItems)
        {
            _items.Clear();
            foreach (var editorItem in editorItems)
            {
                for (int i = 0; i < editorItem.amount; i++)
                {
                    _items.Add(new Item(editorItem.entityIndex, editorItem.health));
                }
            }
            return _items;
        }

        private List<EditorItem> ConvertToEditorItems(List<Item> items)
        {
            _editorItems.Clear();
            foreach (var item in items)
            {
                var itemAlreadyAdded = false;
                foreach (var editorItem in _editorItems)
                {
                    // continue, if index aren't the same
                    if (editorItem.entityIndex != item.entityIndex) continue; 
                    
                    editorItem.amount++;
                    itemAlreadyAdded = true;
                }
                if (!itemAlreadyAdded) _editorItems.Add(new EditorItem(item.health, item.entityIndex));
            }
            return _editorItems;
        }
        
        private void SaveDataToAsset()
        {
            // save data into a temp game asset
            _playerConfig.deck = ConvertToItems(_editorItems);
        
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
            _editorItems = ConvertToEditorItems(_playerConfig.deck);
        }
    
        private void UpdateEntitiesConfig()
        {
            _playerConfig.deck = ConvertToItems(_editorItems);
            _playerConfig.deckLenght = ConvertToItems(_editorItems).Count;
            EditorUtility.SetDirty(_playerConfig);
        }
    }
}