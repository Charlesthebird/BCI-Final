using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ObjectSpawner))]
public class ObjectSpawnerEditor : Editor {

    SerializedProperty objectTypes;
    SerializedProperty lowFreq;
    SerializedProperty highFreq;

    public void OnEnable()
    {
        objectTypes = serializedObject.FindProperty("objectTypes");
        lowFreq = serializedObject.FindProperty("lowFreq");
        highFreq = serializedObject.FindProperty("highFreq");
    }

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        SerializedProperty arraySizeProp = objectTypes.FindPropertyRelative("Array.size");
        //EditorGUILayout.PropertyField(arraySizeProp);

        EditorGUI.indentLevel++;

        for (int i = 0; i < arraySizeProp.intValue; i++)
        {
            EditorGUILayout.PropertyField(objectTypes.GetArrayElementAtIndex(i));
            EditorGUILayout.FloatField("% Low", lowFreq.GetArrayElementAtIndex(i).floatValue);
            EditorGUILayout.FloatField("% High", highFreq.GetArrayElementAtIndex(i).floatValue);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add Object"))
        {
            var t = (ObjectSpawner)target;
        }
    }
}