using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(ShooterLauncher))]
public class ShooterLauncherEditor : Editor
{
    private SerializedProperty shootersProperty;
    private SerializedProperty shooterJsonFilesListProperty;

    private void OnEnable()
    {
        shootersProperty = serializedObject.FindProperty("shooters");
        shooterJsonFilesListProperty = serializedObject.FindProperty("shooterJsonFilesList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ShooterLauncher SL = (ShooterLauncher)target;

        EditorGUILayout.PropertyField(shootersProperty, new GUIContent("Shooters"), true);
        if (GUILayout.Button("Add Shooter"))
        {
            AddShooter(SL);
        }

        if (GUILayout.Button("Remove Last Shooter"))
        {
            RemoveLastShooter(SL);
        }

        EditorGUILayout.LabelField(" ");
        EditorGUILayout.LabelField("Shooter JSON Files", EditorStyles.boldLabel);

        for (int i = 0; i < shooterJsonFilesListProperty.arraySize; i++)
        {
            SerializedProperty shooterJsonFiles = shooterJsonFilesListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("jsonFiles");
            EditorGUILayout.PropertyField(shooterJsonFiles, new GUIContent("Shooter JSON Files "+ i), true);

            if (GUILayout.Button("Add JSON File to Shooter "))
            {
                AddJsonFile(shooterJsonFiles);
            }

            if (GUILayout.Button("Remove JSON File from Shooter "))
            {
                RemoveJsonFile(shooterJsonFiles);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddShooter(ShooterLauncher _sl)
    {
        int shooterCount = _sl.shooters.Count;
        Shooter newShooter = new GameObject("Shooter" + shooterCount).AddComponent<Shooter>();
        newShooter.transform.parent = _sl.transform;
        _sl.shooters.Add(newShooter);
        _sl.shooterJsonFilesList.Add(new ShooterJsonFiles());
        newShooter.muzzle = newShooter.transform;

        newShooter.transform.position = new Vector3(0, 2, 0);
    }

    private void RemoveLastShooter(ShooterLauncher _sl)
    {
        if (_sl.shooters.Count > 0)
        {
            Shooter shooterToRemove = _sl.shooters[_sl.shooters.Count - 1];
            _sl.shooters.RemoveAt(_sl.shooters.Count - 1);
            _sl.shooterJsonFilesList.RemoveAt(_sl.shooterJsonFilesList.Count - 1);
            DestroyImmediate(shooterToRemove.gameObject);
        }
    }

    private void AddJsonFile(SerializedProperty jsonFilesProperty)
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

    private void RemoveJsonFile(SerializedProperty jsonFilesProperty)
    {
        if (jsonFilesProperty.arraySize > 0)
        {
            jsonFilesProperty.DeleteArrayElementAtIndex(jsonFilesProperty.arraySize - 1);
        }
    }
}