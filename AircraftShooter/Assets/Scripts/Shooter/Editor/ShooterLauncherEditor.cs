using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShooterLauncher))]
public class ShooterLauncherEditor : Editor
{
    private SerializedProperty jsonFilesProperty;

    private void OnEnable()
    {
        jsonFilesProperty = serializedObject.FindProperty("jsonFiles");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(jsonFilesProperty, new GUIContent("JSON Files"), true);

        if (GUILayout.Button("Add JSON File"))
        {
            string path = EditorUtility.OpenFilePanel("Select JSON file", Application.dataPath + "Resources/Data", "json");
            if (!string.IsNullOrEmpty(path))
            {
                jsonFilesProperty.arraySize++;
                jsonFilesProperty.GetArrayElementAtIndex(jsonFilesProperty.arraySize - 1).stringValue = path;
            }
        }

        if (GUILayout.Button("Remove Last JSON File"))
        {
            if (jsonFilesProperty.arraySize > 0)
            {
                jsonFilesProperty.arraySize--;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
