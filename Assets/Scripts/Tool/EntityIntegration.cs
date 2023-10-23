using System;
using UnityEditor;
using UnityEngine;

public class EntityIntegration : EditorWindow
{
    string fileName = "FileName";
    
    [MenuItem("Tool/Entity Integration")]
    static void InitializeWindow()
    {
        EntityIntegration window = GetWindow<EntityIntegration>();
        window.titleContent = new GUIContent("Entity Integration");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField ("Text Field", myString);
        
        groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
        {
            myBool = EditorGUILayout.Toggle ("Toggle", myBool);
            myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
        }
        EditorGUILayout.EndToggleGroup ();
    }

    public float myFloat { get; set; }

    public bool myBool { get; set; }

    public bool groupEnabled { get; set; }

    public string myString { get; set; }
}