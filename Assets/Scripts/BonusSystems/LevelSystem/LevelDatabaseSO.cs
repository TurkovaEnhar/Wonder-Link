using System.Collections.Generic;
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
}