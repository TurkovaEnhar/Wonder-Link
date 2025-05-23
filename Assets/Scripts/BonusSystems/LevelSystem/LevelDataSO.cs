﻿using System.Collections.Generic;
using UnityEngine;
using Board.Chips;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Game/Level", fileName = "NewLevel")]
    public class LevelDataSO : ScriptableObject
    {
        [Header("Zorunlu Hedefler")] public int moveCount = 20;
        public int targetScore = 500;

        [Header("İsteğe Bağlı Hedefler")] public List<ChipTarget> colorTargets;
        public LinkTarget linkTarget;

        public bool HasColorTargets => colorTargets != null && colorTargets.Count > 0;
        public bool HasMinLinkTarget => linkTarget.amount > 0;
        public bool autoEndOnTargetCompleted;
    }

    [System.Serializable]
    public class ChipTarget
    {
        public ChipColor color;
        public int count;
    }

    [System.Serializable]
    public class LinkTarget
    {
        public int linkSize;
        public int amount;
    }