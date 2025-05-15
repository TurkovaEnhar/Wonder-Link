using System.Collections.Generic;
using System.IO;
using Board.Chips;
using UnityEngine;

namespace Stats
{
    [System.Serializable]
    public class ChipColorCount
    {
        public ChipColor color;
        public int count;

        public ChipColorCount(ChipColor color, int count)
        {
            this.color = color;
            this.count = count;
        }
    }
    [System.Serializable]
    public class StatSaveData
    {
        public int totalLinks;
        public int maxLinkLength;
        public List<ChipColorCount> chipDestroyMap = new();
    }
    
    public class SaveSystem
    {
        private static string SavePath => Application.persistentDataPath + "/stats.json";

        public static void SaveStats(StatSystem stats)
        {
            StatSaveData data = new StatSaveData
            {
                totalLinks = stats.TotalLinks,
                maxLinkLength = stats.MaxLinkLength,
                chipDestroyMap = new List<ChipColorCount>()
            };

            foreach (var pair in stats.ChipDestroyCount)
            {
                data.chipDestroyMap.Add(new ChipColorCount(pair.Key, pair.Value));
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }

        public static StatSystem LoadStats()
        {
            if (!File.Exists(SavePath))
                return new StatSystem();

            string json = File.ReadAllText(SavePath);
            StatSaveData data = JsonUtility.FromJson<StatSaveData>(json);

            StatSystem stats = new StatSystem();
            stats.TotalLinks = data.totalLinks;
            stats.MaxLinkLength = data.maxLinkLength;

            foreach (var chip in data.chipDestroyMap)
            {
                stats.ChipDestroyCount[chip.color] = chip.count;
            }

            return stats;
        }

        public static void ClearStats()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);
        }
    }
}