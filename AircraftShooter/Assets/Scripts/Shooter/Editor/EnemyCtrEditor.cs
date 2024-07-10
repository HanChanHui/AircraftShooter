using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyCtr))]
public class EnemyCtrEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyCtr enemyCtr = (EnemyCtr)target;

        if (GUILayout.Button("Add Shooter"))
        {
            AddShooter(enemyCtr);
        }

        if (GUILayout.Button("Remove Last Shooter"))
        {
            RemoveLastShooter(enemyCtr);
        }

        // Save changes to the object
        if (GUI.changed)
        {
            EditorUtility.SetDirty(enemyCtr);
        }
    }

    private void AddShooter(EnemyCtr enemyCtr)
    {
        Shooter newShooter = new GameObject("Shooter").AddComponent<Shooter>();
        newShooter.transform.parent = enemyCtr.transform;
        enemyCtr.shooters.Add(newShooter);
        newShooter.muzzle = newShooter.transform;

        newShooter.transform.position = new Vector3(0, 2, 0);
    }

    private void RemoveLastShooter(EnemyCtr enemyCtr)
    {
        if (enemyCtr.shooters.Count > 0)
        {
            Shooter shooterToRemove = enemyCtr.shooters[enemyCtr.shooters.Count - 1];
            enemyCtr.shooters.Remove(shooterToRemove);
            DestroyImmediate(shooterToRemove.gameObject);
        }
    }
}