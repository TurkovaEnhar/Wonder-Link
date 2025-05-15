using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/LevelDatabase", fileName = "LevelDatabase")]
public class LevelDatabaseSO : ScriptableObject
{
    public List<LevelDataSO> levels;

    public int GetLevelCount() => levels.Count;

    public LevelDataSO GetLevel(int index)
    {
        if (index >= 0 && index < levels.Count)
            return levels[index];

        Debug.LogError("Invalid level index!");
        return null;
    }
#if UNITY_EDITOR
    public void AutoFillLevels()
    {
        levels.Clear();

        string[] guids = AssetDatabase.FindAssets("t:LevelDataSO");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            LevelDataSO level = AssetDatabase.LoadAssetAtPath<LevelDataSO>(path);
            if (level != null)
                levels.Add(level);
        }

        EditorUtility.SetDirty(this);
        Debug.Log($"Found and added {levels.Count} LevelSO assets.");
    }
#endif
}