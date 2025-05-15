using System;
using System.Collections.Generic;
using Board.Chips;
using UnityEngine;

namespace Stats
{
    [System.Serializable]
    public class StatSystem
    {
        public int TotalLinks;
        public int MaxLinkLength;
        public Dictionary<ChipColor, int> ChipDestroyCount = new();

        public StatSystem()
        {
            foreach (ChipColor color in Enum.GetValues(typeof(ChipColor)))
                ChipDestroyCount[color] = 0;
        }

        public void RecordLink(int length, ChipColor color)
        {
            TotalLinks++;
            MaxLinkLength = Mathf.Max(MaxLinkLength, length);
            ChipDestroyCount[color] += length;
        }
        
        public void Reset()
        {
            TotalLinks = 0;
            MaxLinkLength = 0;
            foreach (var key in ChipDestroyCount.Keys)
                ChipDestroyCount[key] = 0;
        }
    }
}