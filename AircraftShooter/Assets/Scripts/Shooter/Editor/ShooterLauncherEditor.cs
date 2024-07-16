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
            string path = EditorUtility.OpenFilePanel("Select JSON file", Application.dataPath + "/Resources/Data", "json");
            if (!string.IsNullOrEmpty(path))
            {
                path = "Assets" + path.Replace(Application.dataPath, "").Replace('\\', '/'); // Unity 경로 형식으로 변환
                TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                if (textAsset != null)
                {
                    jsonFilesProperty.arraySize++;
                    jsonFilesProperty.GetArrayElementAtIndex(jsonFilesProperty.arraySize - 1).objectReferenceValue = textAsset;
                }
                else
                {
                    Debug.LogWarning("Failed to load TextAsset from path: " + path);
                }
            }
        }

        if (GUILayout.Button("Remove Last JSON File"))
        {
            if (jsonFilesProperty.arraySize > 0)
            {
                jsonFilesProperty.DeleteArrayElementAtIndex(jsonFilesProperty.arraySize - 1);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
