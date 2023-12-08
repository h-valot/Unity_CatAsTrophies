using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EntityIntegration : EditorWindow
    {
        #region INITIALIZATION
        // SIDE BAR
        private readonly List<EntityConfig> _enemies = new List<EntityConfig>();
        private readonly List<EntityConfig> _cats = new List<EntityConfig>();

        // INFORMATIONS
        private EntityConfig _currentEntity;

        // global infos
        private string _entityName;
        private float _health;
        private int _armorAtStart;
        private Sprite _sprite;
        
        // abilities
        private Ability _ability;
        private List<Ability> _autoAttacks = new List<Ability>();
        
        // graphics
        private GameObject _basePrefab, _rightHandAddon, _leftHandAddon, _headAddon;
        private Texture _catSkinTexture;
        private Texture _catEyesTexture;

        // rewards
        private bool _canBeReward;
        private string _rewardName;
        private List<CompositionTier> _rewardApparitionTiers = new List<CompositionTier>();
        private RewardPricing _rewardPricing;
        private float _rewardCost;
        
        // window editor component
        private int _id;
        private Vector2 _sideBarScroll, _informationsScroll;
        private bool _canDisplayDetails = true;
        private EntitiesConfig _entitiesConfig;
        #endregion
    
        [MenuItem("Integration Tools/Entity")]
        static void InitializeWindow()
        {
            EntityIntegration window = GetWindow<EntityIntegration>();
            window.titleContent = new GUIContent("Entity Integration");
            window.maxSize = new Vector2(801, 421);
            window.minSize = new Vector2(800, 420);
            window.Show();
        }
    
        private void OnEnable()
        {
            _cats.Clear();
            _enemies.Clear();
        
            // load the data from the entities config
            LoadDataFromAsset();
        
            if (_cats.Count > 0)
            {
                UpdateInformations(_cats[0]);
            }
            else if (_enemies.Count > 0)
            {
                UpdateInformations(_enemies[0]);
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
                EditorGUILayout.BeginVertical(GUILayout.Width(150), GUILayout.ExpandHeight(true));
                {
                    GUILayout.BeginVertical("HelpBox");
                    {
                        _sideBarScroll = EditorGUILayout.BeginScrollView(_sideBarScroll);
                        {
                            DisplaySideBar();
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();
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
            #region CATS SIDE LIST
            // HEADER
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("CATS", EditorStyles.boldLabel);
                if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    var newInstance = CreateInstance<EntityConfig>();
                    newInstance.isCat = true;
                    newInstance.Initialize();
                    var date = DateTime.Now;
                    newInstance.id = "Cat_" + date.ToString("yyyyMMdd_HHmmss_fff");
                
                    _cats.Add(newInstance);
                    UpdateInformations(newInstance);
                    _canDisplayDetails = true;
                }
            }
            GUILayout.EndHorizontal();
        
            GUILayout.Space(5);
    
            // LIST OF CATS
            foreach (EntityConfig cat in _cats)
            {
                string buttonName = cat.entityName == "" ? "New cat" : cat.entityName;
                if (GUILayout.Button($"{buttonName}", GUILayout.ExpandWidth(true), GUILayout.Height(20)))
                {
                    UpdateInformations(cat);
                }
            }
            #endregion
            #region ENEMIES SIDE LIST
            // HEADER
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("ENEMIES", EditorStyles.boldLabel);
                if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    var newInstance = CreateInstance<EntityConfig>();
                    newInstance.isCat = false;
                    var date = DateTime.Now;
                    newInstance.id = "Enemy_" + date.ToString("yyyyMMdd_HHmmss_fff");
                
                    _enemies.Add(newInstance);
                    UpdateInformations(newInstance);
                    _canDisplayDetails = true;
                }
            }
            GUILayout.EndHorizontal();
    
            GUILayout.Space(5);
        
            // LIST OF ENEMIES
            foreach (EntityConfig enemy in _enemies)
            {
                string buttonName = enemy.entityName == "" ? "New enemy" : enemy.entityName;
                if (GUILayout.Button($"{buttonName}", GUILayout.ExpandWidth(true), GUILayout.Height(20)))
                {
                    UpdateInformations(enemy);
                }
            }
            #endregion
        }
    
        private void DisplayInformations()
        {
            // INFORMATIONS
            GUILayout.Label("INFORMATION", EditorStyles.boldLabel);
            _entityName = EditorGUILayout.TextField("Name", _entityName);
            _health = EditorGUILayout.FloatField("Health", _health);
            _armorAtStart = EditorGUILayout.IntField("Armor at start", _armorAtStart);
            _sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", _sprite, typeof(Sprite), true);
    
        
            // ABILITY
            GUILayout.Space(10);
            GUILayout.Label("ABILITY", EditorStyles.boldLabel);
            DisplayInstructionList(_ability);
        

            // AUTO ATTACK
            GUILayout.Space(10);
            GUILayout.Label("AUTO ATTACK", EditorStyles.boldLabel);
            DisplayAbilityList(_autoAttacks);
        

            // GRAPHICS
            GUILayout.Space(10);
            GUILayout.Label("GRAPHICS", EditorStyles.boldLabel);
            _basePrefab = (GameObject)EditorGUILayout.ObjectField("Base mesh prefab", _basePrefab, typeof(GameObject), true);
            _catSkinTexture = (Texture)EditorGUILayout.ObjectField("Cat skin", _catSkinTexture, typeof(Texture), true);
            _catEyesTexture = (Texture)EditorGUILayout.ObjectField("Cat eyes texture", _catEyesTexture, typeof(Texture), true);
            _rightHandAddon = (GameObject)EditorGUILayout.ObjectField("Right hand addon", _rightHandAddon, typeof(GameObject), true);
            _leftHandAddon = (GameObject)EditorGUILayout.ObjectField("Left hand addon", _leftHandAddon, typeof(GameObject), true);
            _headAddon = (GameObject)EditorGUILayout.ObjectField("Head addon", _headAddon, typeof(GameObject), true);


            // REWARD
            if (_currentEntity.isCat)
            {
                GUILayout.Space(10);
                GUILayout.Label("REWARD", EditorStyles.boldLabel);
                _canBeReward = EditorGUILayout.Toggle("Can this cat be a reward", _canBeReward);
                if (_canBeReward)
                {
                    GUILayout.Label("Apparition settings");
                    DisplayApparitionTierChoser();
                    
                    GUILayout.Label("Pricing settings");
                    _rewardPricing = (RewardPricing)EditorGUILayout.EnumPopup("Pricing", _rewardPricing);
                    if (_rewardPricing == RewardPricing.PREMIUM) _rewardCost = EditorGUILayout.FloatField("Cost", _rewardCost);
                    else _rewardCost = 0;
                }
            }
            
        
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
                    DeleteData();
                }
            }
            GUI.backgroundColor = Color.white;
        }

        private void UpdateInformations(EntityConfig entityConfig)
        {
            _currentEntity = entityConfig;
        
            // INFORMATIONS
            _entityName = _currentEntity.entityName;
            _health = _currentEntity.health;
            _armorAtStart = _currentEntity.armorAtStart;
            _sprite = _currentEntity.sprite;

            // ABILITY
            _ability = _currentEntity.ability;
            _autoAttacks = _currentEntity.autoAttack;

            // GRAPHICS
            _basePrefab = _currentEntity.basePrefab;
            _catSkinTexture = _currentEntity.catSkinTexture;
            _catEyesTexture = _currentEntity.catEyesTexture;
            _rightHandAddon = _currentEntity.rightHandAddon;
            _leftHandAddon = _currentEntity.leftHandAddon;
            _headAddon = _currentEntity.headAddon;
            
            // REWARD
            _canBeReward = _currentEntity.canBeReward;
            _rewardApparitionTiers = _currentEntity.apparitionTiers;
            _rewardPricing = _currentEntity.pricing;
            _rewardCost = _currentEntity.cost;
        }
        
        private void DisplayAbilityList(List<Ability> autoAttack)
        {
            GUILayout.BeginVertical("HelpBox");
            {
                // HEADER
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Abilities");
                    if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                    {
                        var newAutoAttackAbility = new Ability();
                        newAutoAttackAbility.instructions.Add(new Instruction());
                        autoAttack.Add(newAutoAttackAbility);
                    }
                }
                GUILayout.EndHorizontal();

                // LIST
                foreach (Ability autoAttackAbility in autoAttack)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.BeginVertical();
                        {
                            DisplayInstructionList(autoAttackAbility);
                        }
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical();
                        {
                            GUI.backgroundColor = Color.red;
                            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                if (EditorUtility.DisplayDialog("Delete Ability", "Do you really want to permanently delete this Ability?", "Yes", "No"))
                                {
                                    autoAttack.Remove(autoAttackAbility);
                                    return;
                                }
                            }
                            GUI.backgroundColor = Color.white;
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }

        private void DisplayInstructionList(Ability ability)
        {
            GUILayout.BeginVertical("HelpBox");
            {
                ability.animation = (AbilityAnimation)EditorGUILayout.EnumPopup("Animation type", ability.animation);
            
                // HEADER
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Instructions");
                    if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                    {
                        var newInstruction = new Instruction();
                        ability.instructions.Add(newInstruction);
                    }
                }
                GUILayout.EndHorizontal();

                // LIST
                if (ability != null)
                {
                    foreach (Instruction instruction in ability.instructions)
                    {
                        GUILayout.BeginHorizontal("HelpBox");
                        {
                            GUILayout.BeginVertical();
                            {
                                instruction.type = (InstructionType)EditorGUILayout.EnumPopup("Type", instruction.type);
                                instruction.target = (TargetType)EditorGUILayout.EnumPopup("Target", instruction.target);
                            }
                            GUILayout.EndVertical();
                            GUILayout.BeginVertical();
                            {
                                instruction.value = EditorGUILayout.IntField("Value", instruction.value);
                                instruction.us = EditorGUILayout.Toggle("Us", instruction.us);
                            }
                            GUILayout.EndVertical();
                            GUILayout.BeginVertical();
                            {
                                GUI.backgroundColor = Color.red;
                                if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                                {
                                    if (EditorUtility.DisplayDialog("Delete Instruction", "Do you really want to permanently delete this instruction?", "Yes", "No"))
                                    {
                                        ability.instructions.Remove(instruction);
                                        return;
                                    }
                                }
                                GUI.backgroundColor = Color.white;
                            }
                            GUILayout.EndVertical();
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUILayout.EndVertical();
        }
    
        private void DisplayApparitionTierChoser()
        {
            GUILayout.BeginVertical("HelpBox");
            {
                // HEADER
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Apparition tiers");
                    if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                    {
                        _rewardApparitionTiers.Add(CompositionTier.SIMPLE);
                    }
                }
                GUILayout.EndHorizontal();

                for (int index = 0; index < _rewardApparitionTiers.Count; index++)
                {
                    DisplayApparitionTier(index);
                }
            }
            GUILayout.EndVertical();
        }

        private void DisplayApparitionTier(int index)
        {
            GUILayout.BeginHorizontal("HelpBox");
            {
                _rewardApparitionTiers[index] = (CompositionTier)EditorGUILayout.EnumPopup(_rewardApparitionTiers[index]);
                        
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    if (EditorUtility.DisplayDialog("Delete apparition tier", "Do you really want to permanently delete this apparition tier?", "Yes", "No"))
                    {
                        _rewardApparitionTiers.Remove(_rewardApparitionTiers[index]);
                    }
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
        }

        private void SaveDataToAsset()
        {
            // exceptions
            if (_entityName == "") return;
            
            // base infos
            _currentEntity.entityName = _entityName;
            _currentEntity.health = _health;
            _currentEntity.armorAtStart = _armorAtStart;
            _currentEntity.sprite = _sprite;
            
            // ability
            _currentEntity.ability = _ability;
            _currentEntity.autoAttack = _autoAttacks;
            
            // graphics
            _currentEntity.basePrefab = _basePrefab;
            _currentEntity.catSkinTexture = _catSkinTexture;
            _currentEntity.catEyesTexture = _catEyesTexture;
            _currentEntity.rightHandAddon = _rightHandAddon;
            _currentEntity.leftHandAddon = _leftHandAddon;
            _currentEntity.headAddon = _headAddon;
            
            // reward
            _currentEntity.canBeReward = _canBeReward;
            _currentEntity.apparitionTiers = _rewardApparitionTiers;
            _currentEntity.pricing = _rewardPricing;
            _currentEntity.cost = _rewardCost;

            // get the path
            string path = $"Assets/Configs/Entities/{_currentEntity.id}.asset";

            // if the asset does not already exists then create a new one 
            if (!AssetDatabase.LoadAssetAtPath<EntityConfig>(path))
            {
                AssetDatabase.CreateAsset(_currentEntity, path);
            }
        
            // save changes on the entityConfig
            EditorUtility.SetDirty(_currentEntity);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        
            // update entities config
            UpdateEntitiesConfig();
        }

        private void DeleteData()
        {
            // remove from lists depending on the entity
            if (_currentEntity.isCat)
            {
                _cats.Remove(_currentEntity);
            }
            else
            {
                _enemies.Remove(_currentEntity);
            }

            // deleting the asset
            AssetDatabase.DeleteAsset($"Assets/Configs/Entities/{_currentEntity.id}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        
            // update entities config
            UpdateEntitiesConfig();
        }
    
        private void LoadDataFromAsset()
        {
            // load entities config
            _entitiesConfig = EditorMisc.FindEntitiesConfig();
        
            // get all files with type "EntityConfig" in the project
            string[] fileGuidsArray = AssetDatabase.FindAssets("t:" + typeof(EntityConfig));

            // fullfil cats and enemies with EntityConfigs 
            foreach (string fileGuid in fileGuidsArray)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(fileGuid);
                EntityConfig entityConfig = AssetDatabase.LoadAssetAtPath<EntityConfig>(assetPath);

                if (entityConfig.isCat)
                {
                    _cats.Add(entityConfig);
                }
                else
                {
                    _enemies.Add(entityConfig);
                }
            }
        }

        private void UpdateEntitiesConfig()
        {
            _entitiesConfig = EditorMisc.FindEntitiesConfig();
            _entitiesConfig.cats = _cats;
            _entitiesConfig.enemies = _enemies;
            EditorUtility.SetDirty(_entitiesConfig);
        }
    }
}