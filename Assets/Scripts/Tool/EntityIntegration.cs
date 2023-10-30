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
    private Ability[] autoAttack;
    private GameObject basePrefab, rightHandAddon, leftHandAddon, headAddon;
    
    // window editor component
    private int id;
    private Vector2 sideScrollPos, detailsScrollPos;
    private bool canDisplayDetails = true;
    #endregion
    
    [MenuItem("Tool/Entity Integration")]
    static void InitializeWindow()
    {
        EntityIntegration window = GetWindow<EntityIntegration>();
        window.titleContent = new GUIContent("Entity Integration");
        window.Show();
    }
    
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(150), GUILayout.ExpandHeight(true));
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
        cats.Clear();
        enemies.Clear();
        
        // load the data from the entities config
        LoadData();
        
        if (cats.Count > 0)
        {
            UpdateDetails(cats[0]);
        }
        else if (enemies.Count > 0)
        {
            UpdateDetails(enemies[0]);
        }
        else
        {
            canDisplayDetails = false;
        }

        OnSelectionChange();
        InitList();
    }

    private SerializedObject serializedObject;
    private SerializedProperty SomeClasses;
    private ReorderableList list;

    private Dictionary<string, ReorderableList> innerListDict = new Dictionary<string, ReorderableList>();

    private void OnSelectionChange()
    {
        // Get editable `SomeBehaviour` objects from selection.
        var filtered = Selection.GetFiltered(typeof(Ability), SelectionMode.Editable);
        if (filtered.Length == 0) {
            serializedObject = null;
            SomeClasses = null;
        }
        else {
            // Let's work with the first filtered result.
            serializedObject = new SerializedObject(filtered[0]);
            SomeClasses = serializedObject.FindProperty("wishlist");
        }

        Repaint();
    }
    
    private void InitList()
    {
        SomeClasses = serializedObject.FindProperty("instructions");

        // setupt the outer list
        list = new ReorderableList(serializedObject, SomeClasses)
        {
            displayAdd = true,
            displayRemove = true,
            draggable = true,

            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Outer List");
            },

            drawElementCallback = (rect, index, a, h) =>
            {
                // get outer element
                var element = SomeClasses.GetArrayElementAtIndex(index);

                var InnerList = element.FindPropertyRelative("InnerList");

                string listKey = element.propertyPath;

                ReorderableList innerReorderableList;

                if (innerListDict.ContainsKey(listKey))
                {
                    // fetch the reorderable list in dict
                    innerReorderableList = innerListDict[listKey];
                }
                else
                {
                    // create reorderabl list and store it in dict
                    innerReorderableList = new ReorderableList(element.serializedObject, InnerList)
                    {
                        displayAdd = true,
                        displayRemove = true,
                        draggable = true,

                        drawHeaderCallback = innerRect =>
                        {
                            EditorGUI.LabelField(innerRect, "Inner List");
                        },

                        drawElementCallback = (innerRect, innerIndex, innerA, innerH) =>
                        {
                            // Get element of inner list
                            var innerElement = InnerList.GetArrayElementAtIndex(innerIndex);

                            var name = innerElement.FindPropertyRelative("Name");

                            EditorGUI.PropertyField(innerRect, name);
                        }
                    };
                    innerListDict[listKey] = innerReorderableList;
                }

                // Setup the inner list
                var height = (InnerList.arraySize + 3) * EditorGUIUtility.singleLineHeight;
                innerReorderableList.DoList(new Rect(rect.x, rect.y, rect.width, height));
            },

            elementHeightCallback = index =>
            {
                var element = SomeClasses.GetArrayElementAtIndex(index);

                var innerList = element.FindPropertyRelative("InnerList");

                return (innerList.arraySize + 4) * EditorGUIUtility.singleLineHeight;
            }
        };
    }
    
    private void OnDisable()
    {
        UpdateEntitiesConfig();
    }
    
    private void DisplaySideLists()
    {
        GUILayout.BeginVertical();
        {
            sideScrollPos = EditorGUILayout.BeginScrollView(sideScrollPos);
            #region CATS SIDE LIST
            // HEADER
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("CATS", EditorStyles.boldLabel);
                if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    var newInstance = CreateInstance<EntityConfig>();
                    newInstance.isCat = true;
                    cats.Add(newInstance);
                    UpdateDetails(newInstance);
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
                    UpdateDetails(cat);
                }
            }
            #endregion
            GUILayout.Space(15);
            #region ENEMIES SIDE LIST
            // HEADER
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("ENEMIES", EditorStyles.boldLabel);
                if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    var newInstance = CreateInstance<EntityConfig>();
                    newInstance.isCat = false;
                    enemies.Add(newInstance);
                    UpdateDetails(newInstance);
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
                    UpdateDetails(enemy);
                }
            }
            #endregion
            EditorGUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
    }
    
    private void DisplayDetails()
    {
        detailsScrollPos = EditorGUILayout.BeginScrollView(detailsScrollPos);
        {
            // INFORMATIONS
            GUILayout.Label("INFORMATION", EditorStyles.boldLabel);
            entityName = EditorGUILayout.TextField("Name", entityName);
            health = EditorGUILayout.FloatField("Health", health);
        
            
            // ABILITY
            GUILayout.Space(10);
            GUILayout.Label("ABILITY", EditorStyles.boldLabel);
            // isn't completed
            
            
            // AUTO ATTACK
            GUILayout.Space(10);
            GUILayout.Label("AUTO ATTACK", EditorStyles.boldLabel);

            if (serializedObject != null)
            {
                serializedObject.Update();
                list.DoLayoutList();
                serializedObject.ApplyModifiedProperties();
            }
            
            
            // GRAPHICS
            GUILayout.Space(10);
            GUILayout.Label("GRAPHICS", EditorStyles.boldLabel);
            basePrefab = (GameObject)EditorGUILayout.ObjectField("Base mesh prefab", basePrefab, typeof(GameObject), true);
            rightHandAddon = (GameObject)EditorGUILayout.ObjectField("Right hand addon", rightHandAddon, typeof(GameObject), true);
            leftHandAddon = (GameObject)EditorGUILayout.ObjectField("Left hand addon", leftHandAddon, typeof(GameObject), true);
            headAddon = (GameObject)EditorGUILayout.ObjectField("Head addon", headAddon, typeof(GameObject), true);

            
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
        if (entityName == "") return;
            
        // update data into current entity
        currentEntity.entityName = entityName;
        currentEntity.health = health;
        currentEntity.ability = ability;
        currentEntity.autoAttack = autoAttack;
        currentEntity.basePrefab = basePrefab;
        currentEntity.rightHandAddon = rightHandAddon;
        currentEntity.leftHandAddon = leftHandAddon;
        currentEntity.headAddon = headAddon;

        // get the path depending on the entity
        string path = currentEntity.isCat 
            ? $"Assets/Configs/Cats/Cat_{currentEntity.entityName}.asset" 
            : $"Assets/Configs/Enemies/Enemy_{currentEntity.entityName}.asset";

        // if the asset does not already exists then create a new one 
        if (!AssetDatabase.LoadAssetAtPath<EntityConfig>(path))
        {
            AssetDatabase.CreateAsset(currentEntity, path);
        }
        
        // save changes
        AssetDatabase.SaveAssets();
    }

    private void DeleteData()
    {
        string path; 
            
        // get path and remove from lists depending on the entity
        if (currentEntity.isCat)
        {
            cats.Remove(currentEntity);
            path = $"Assets/Configs/Cats/Cat_{currentEntity.entityName}.asset";
        }
        else
        {
            enemies.Remove(currentEntity);
            path = $"Assets/Configs/Enemies/Enemy_{currentEntity.entityName}.asset";
        }

        // deleting the asset
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private void LoadData()
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
    
    private void UpdateDetails(EntityConfig _entityConfig)
    {
        currentEntity = _entityConfig;
        
        // INFORMATIONS
        entityName = currentEntity.entityName;
        health = currentEntity.health;

        // ABILITY
        ability = currentEntity.ability;
        autoAttack = currentEntity.autoAttack;

        // GRAPHICS
        basePrefab = currentEntity.basePrefab;
        rightHandAddon = currentEntity.rightHandAddon;
        leftHandAddon = currentEntity.leftHandAddon;
        headAddon = currentEntity.headAddon;
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
        FindEntitiesConfig().cats.Clear();
        FindEntitiesConfig().cats = cats;
        FindEntitiesConfig().enemies.Clear();
        FindEntitiesConfig().enemies = enemies;
    }
}