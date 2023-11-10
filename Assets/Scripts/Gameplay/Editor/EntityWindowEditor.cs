using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class EntityIntegration : EditorWindow
{
    #region INITIALIZATION
    // lists of entities
    private List<EntityConfig> enemies = new List<EntityConfig>();
    private List<EntityConfig> cats = new List<EntityConfig>();

    // entities attributes
    private EntityConfig currentEntity;

    private string entityName;
    private float health;
    private Ability ability;
    private List<Ability> autoAttacks = new List<Ability>();
    private GameObject basePrefab, rightHandAddon, leftHandAddon, headAddon;
    private Material baseMaterial;
    
    // window editor component
    private int id;
    private Vector2 sideBarScroll, informationsScroll;
    private bool canDisplayDetails = true;
    private ReorderableList reorderableList;
    #endregion
    
    [MenuItem("Tool/Entity")]
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
        cats.Clear();
        enemies.Clear();
        
        // load the data from the entities config
        LoadDataFromAsset();
        
        if (cats.Count > 0)
        {
            UpdateInformations(cats[0]);
        }
        else if (enemies.Count > 0)
        {
            UpdateInformations(enemies[0]);
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
            EditorGUILayout.BeginVertical(GUILayout.Width(150), GUILayout.ExpandHeight(true));
            {
                GUILayout.BeginVertical("HelpBox");
                {
                    sideBarScroll = EditorGUILayout.BeginScrollView(sideBarScroll);
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
        #region CATS SIDE LIST
        // HEADER
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("CATS", EditorStyles.boldLabel);
            if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
            {
                var newInstance = CreateInstance<EntityConfig>();
                newInstance.isCat = true;
                var date = DateTime.Now;
                newInstance.id = "Cat_" + date.ToString("yyyyMMdd_HHmmss_fff");
                
                cats.Add(newInstance);
                UpdateInformations(newInstance);
                canDisplayDetails = true;
            }
        }
        GUILayout.EndHorizontal();
        
        GUILayout.Space(5);
    
        // LIST OF CATS
        foreach (EntityConfig cat in cats)
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
                
                enemies.Add(newInstance);
                UpdateInformations(newInstance);
                canDisplayDetails = true;
            }
        }
        GUILayout.EndHorizontal();
    
        GUILayout.Space(5);
        
        // LIST OF ENEMIES
        foreach (EntityConfig enemy in enemies)
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
        entityName = EditorGUILayout.TextField("Name", entityName);
        health = EditorGUILayout.FloatField("Health", health);
    
        
        // ABILITY
        GUILayout.Space(10);
        GUILayout.Label("ABILITY", EditorStyles.boldLabel);
        DisplayInstructionList(ability);
        

        // AUTO ATTACK
        GUILayout.Space(10);
        GUILayout.Label("AUTO ATTACK", EditorStyles.boldLabel);
        DisplayAbilityList(autoAttacks);
        

        // GRAPHICS
        GUILayout.Space(10);
        GUILayout.Label("GRAPHICS", EditorStyles.boldLabel);
        basePrefab = (GameObject)EditorGUILayout.ObjectField("Base mesh prefab", basePrefab, typeof(GameObject), true);
        baseMaterial = (Material)EditorGUILayout.ObjectField("Cat skin", baseMaterial, typeof(Material), true);
        rightHandAddon = (GameObject)EditorGUILayout.ObjectField("Right hand addon", rightHandAddon, typeof(GameObject), true);
        leftHandAddon = (GameObject)EditorGUILayout.ObjectField("Left hand addon", leftHandAddon, typeof(GameObject), true);
        headAddon = (GameObject)EditorGUILayout.ObjectField("Head addon", headAddon, typeof(GameObject), true);

        
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

    private void UpdateInformations(EntityConfig _entityConfig)
    {
        currentEntity = _entityConfig;
        
        // INFORMATIONS
        entityName = currentEntity.entityName;
        health = currentEntity.health;

        // ABILITY
        ability = currentEntity.ability;
        autoAttacks = currentEntity.autoAttack;

        // GRAPHICS
        basePrefab = currentEntity.basePrefab;
        baseMaterial = currentEntity.baseMaterial;
        rightHandAddon = currentEntity.rightHandAddon;
        leftHandAddon = currentEntity.leftHandAddon;
        headAddon = currentEntity.headAddon;
    }

    private void DisplayAbilityList(List<Ability> _autoAttack)
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
                    _autoAttack.Add(newAutoAttackAbility);
                }
            }
            GUILayout.EndHorizontal();

            // LIST
            foreach (Ability autoAttackAbility in _autoAttack)
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
                                _autoAttack.Remove(autoAttackAbility);
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

    private void DisplayInstructionList(Ability _ability)
    {
        GUILayout.BeginVertical("HelpBox");
        {
            _ability.animation = (AbilityAnimation)EditorGUILayout.EnumPopup("Animation type", _ability.animation);
            
            // HEADER
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Instructions");
                if (GUILayout.Button("Add", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    var newInstruction = new Instruction();
                    _ability.instructions.Add(newInstruction);
                }
            }
            GUILayout.EndHorizontal();

            // LIST
            if (_ability != null)
            {
                foreach (Instruction instruction in _ability.instructions)
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
                                    _ability.instructions.Remove(instruction);
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
    
    private void SaveDataToAsset()
    {
        // exceptions
        if (entityName == "") return;
            
        // update data into current entity
        currentEntity.entityName = entityName;
        currentEntity.health = health;
        currentEntity.ability = ability;
        currentEntity.autoAttack = autoAttacks;
        currentEntity.basePrefab = basePrefab;
        currentEntity.baseMaterial = baseMaterial; 
        currentEntity.rightHandAddon = rightHandAddon;
        currentEntity.leftHandAddon = leftHandAddon;
        currentEntity.headAddon = headAddon;

        // get the path
        string path = $"Assets/Configs/Entities/{currentEntity.id}.asset";

        // if the asset does not already exists then create a new one 
        if (!AssetDatabase.LoadAssetAtPath<EntityConfig>(path))
        {
            AssetDatabase.CreateAsset(currentEntity, path);
        }
        
        // save changes
        EditorUtility.SetDirty(currentEntity);
        EditorUtility.SetDirty(EditorMisc.FindEntitiesConfig());
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void DeleteData()
    {
        // remove from lists depending on the entity
        if (currentEntity.isCat)
        {
            cats.Remove(currentEntity);
        }
        else
        {
            enemies.Remove(currentEntity);
        }

        // deleting the asset
        AssetDatabase.DeleteAsset($"Assets/Configs/Entities/{currentEntity.id}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private void LoadDataFromAsset()
    {
        // get all files with type "EntityConfig" in the project
        string[] fileGuidsArray = AssetDatabase.FindAssets("t:" + typeof(EntityConfig));

        // fullfil cats and enemies with EntityConfigs 
        foreach (string fileGuid in fileGuidsArray)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuid);
            EntityConfig entityConfig = AssetDatabase.LoadAssetAtPath<EntityConfig>(assetPath);

            if (entityConfig.isCat)
            {
                cats.Add(entityConfig);
            }
            else
            {
                enemies.Add(entityConfig);
            }
        }
    }
    
    private void UpdateEntitiesConfig()
    {
        var entitiesConfig = EditorMisc.FindEntitiesConfig();
        entitiesConfig.cats = cats;
        entitiesConfig.enemies = enemies;
        EditorUtility.SetDirty(entitiesConfig);
    }
}