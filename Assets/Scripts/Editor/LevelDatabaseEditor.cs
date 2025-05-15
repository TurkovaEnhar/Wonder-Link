using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelDatabaseSO))]
public class LevelDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelDatabaseSO database = (LevelDatabaseSO)target;

        if (GUILayout.Button("Find All Levels"))
        {
            database.AutoFillLevels();
        }
    }
}