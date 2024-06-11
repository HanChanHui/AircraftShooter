using UnityEditor;
using UnityEditor.Compilation;

[InitializeOnLoad]
public class AutoRefresh
{
    static AutoRefresh()
    {
        EditorApplication.update += Update;
    }

    private static void Update()
    {
        if (!EditorApplication.isCompiling)
        {
            AssetDatabase.Refresh();
            EditorApplication.update -= Update;
        }
    }
}
