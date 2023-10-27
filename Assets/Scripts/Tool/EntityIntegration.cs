using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityIntegration : EditorWindow
{
    #region INITIALIZATION
    
    // lists of entities
    private List<EntityConfig> enemies;
    private List<EntityConfig> cats;

    // entities attributes
    private string name;
    private int health;
    private Ability ability;
    private List<Ability> autoAttack;
    private GameObject basePrefab, rightHandAddon, leftHandAddon, headAddon;
    
    // window editor component

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
                DisplayList();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            {
                DisplayDetails();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DisplayList()
    {
        GUILayout.Label("Cats", EditorStyles.boldLabel);
        GUILayout.Label("Enemies", EditorStyles.boldLabel);
    }

    private void DisplayDetails()
    {
        // ENTITY INFOS
        GUILayout.Label("Entity infos", EditorStyles.boldLabel);
        
        // get entity's name and health
        name = EditorGUILayout.TextField("Name", name);
        health = EditorGUILayout.IntField("Health", health);
        
        // get entity's abilities
        
        
        // ENTITY MESHES
        GUILayout.Space(10);
        GUILayout.Label("Entity meshes", EditorStyles.boldLabel);
        
        // get entity's meshes references
        basePrefab = (GameObject)EditorGUILayout.ObjectField("Base mesh prefab", basePrefab, typeof(GameObject), true);
        rightHandAddon = (GameObject)EditorGUILayout.ObjectField("Right hand addon", rightHandAddon, typeof(GameObject), true);
        leftHandAddon = (GameObject)EditorGUILayout.ObjectField("Left hand addon", leftHandAddon, typeof(GameObject), true);
        headAddon = (GameObject)EditorGUILayout.ObjectField("Head addon", headAddon, typeof(GameObject), true);
    }
}