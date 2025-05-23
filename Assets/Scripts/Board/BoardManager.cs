﻿using System;
using System.Collections;
using System.Collections.Generic;
using Board.Chips;
using DG.Tweening;
using Game;
using Link;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Board
{
    public class BoardManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Chip chipPrefab;
        [SerializeField] private Sprite tileSprite;
        [Header("Settings")]
        [SerializeField] private ChipSettings chipVisualConfig;
        [SerializeField] private float spacing = 0.1f;
        private int initialPoolSize = 100;
        
        private Tile[,] _board;
        private GameConfig _boardSettings;
        private LinkService _linkService;
        private BoardScanService _boardScanService;
        private ObjectPool<Chip> _chipPool;


        public void Initialize( LinkService linkService,BoardScanService boardScanService, GameConfig gameConfig )
        {
            _boardSettings = gameConfig;
            _linkService = linkService;
            _boardScanService = boardScanService;
            _chipPool = new ObjectPool<Chip>(chipPrefab, initialPoolSize, transform);
            GenerateBoard();

            _linkService.OnLinkSuccess += Fill;
        }
        public void GenerateBoard()
        {
            int width = _boardSettings.GetBoardWidth();
            int height = _boardSettings.GetBoardHeight();
            _board = new Tile[width, height];
            Vector2 tileSize = GetTileSize();
            Vector2 offset = new Vector2(
                (width - 1) * tileSize.x / 2f,
                (height - 1) * tileSize.y / 2f
            );

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 position = new Vector3(x * tileSize.x, y * tileSize.y, 0) - (Vector3)offset;
                    GameObject tileGO = Instantiate(tilePrefab, position, Quaternion.identity, transform);

                    Tile tile = tileGO.GetComponent<Tile>();
                    tile.Initialize(x, y);
                    _board[x, y] = tile;

                    SpawnChip(tile);
                }
            }
        }
        
        
        private void SpawnChip(Tile tile)
        {
            Vector3 spawnPos = tile.transform.position + Vector3.up * 2f;

            Chip chip = _chipPool.Get();
            chip.transform.SetParent(tile.transform,false);
            chip.transform.position = spawnPos;
           
            ChipColor randomColor = GetRandomColor();
            Sprite chipSprite = chipVisualConfig.GetSpriteForColor(randomColor);

            chip.Initialize(randomColor, tile, chipSprite);
            chip.OnReturnToPool = _chipPool.Return;

            tile.SetChip(chip);
            chip.ResetColor();
            chip.transform.DOMove(tile.transform.position, 0.5f).SetEase(Ease.OutBack);
        }

  
        
        private void Fill()
        {
            FillBoard();

            bool hasMoves = _boardScanService.HasPossibleMoves(GetBoard(), _boardSettings.linkMode);
            if (!hasMoves)
            {
                ShuffleBoard();
            }
        }
        private void FillBoard()
        {
            int width = _board.GetLength(0);
            int height = _board.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                FillColumn(x, height);
            }
        }
   
        private void FillColumn(int x, int height)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = _board[x, y];
                if (tile.CurrentChip == null)
                {
                    Chip chipToMove = FindChipAbove(x, y);
                    if (chipToMove != null)
                    {
                        MoveChipToTile(chipToMove, tile);
                    }
                    else
                    {
                        SpawnChip(tile);
                    }
                }
            }
        }
        private void MoveChipToTile(Chip chip, Tile targetTile)
        {
            Tile originTile = chip.ParentTile;

            originTile.ClearChip();
            targetTile.SetChip(chip);
            chip.ParentTile = targetTile;
            chip.transform.DOMove(targetTile.transform.position, 0.25f).SetEase(Ease.OutBack);
        }
        private Chip FindChipAbove(int x, int startY)
        {
            int height = _board.GetLength(1);

            for (int y = startY + 1; y < height; y++)
            {
                Tile upperTile = _board[x, y];
                if (upperTile.CurrentChip != null)
                {
                    return upperTile.CurrentChip;
                }
            }

            return null;
        }

        private void ShuffleBoard()
        {
            List<Chip> chips = new();
            
            foreach (Tile tile in _board)
            {
                if (tile.CurrentChip != null)
                {
                    chips.Add(tile.CurrentChip);
                    tile.ClearChip();
                }
            }
            
            for (int i = 0; i < chips.Count; i++)
            {
                Chip temp = chips[i];
                int rand = Random.Range(i, chips.Count);
                chips[i] = chips[rand];
                chips[rand] = temp;
            }
            
            int index = 0;
            for (int x = 0; x < _board.GetLength(0); x++)
            {
                for (int y = 0; y < _board.GetLength(1); y++)
                {
                    Tile tile = _board[x, y];
                    if (index >= chips.Count) return;

                    Chip chip = chips[index++];
                    chip.transform.position = tile.transform.position;
                    tile.SetChip(chip);
                    chip.ParentTile = tile;
                }
            }
            bool hasMoves = _boardScanService.HasPossibleMoves(GetBoard(), _boardSettings.linkMode);
            if (!hasMoves)
            {
                ShuffleBoard();
            }
        }

        private Vector2 GetTileSize()
        {
            if (tileSprite == null)
            {
                return Vector2.one;
            }

            float pixelsPerUnit = tileSprite.pixelsPerUnit;
            Vector2 sizeInUnits = tileSprite.rect.size / pixelsPerUnit;
            return sizeInUnits + Vector2.one * spacing;
        }

        private ChipColor GetRandomColor()
        {
            return (ChipColor)Random.Range(0, System.Enum.GetValues(typeof(ChipColor)).Length);
        }
        
        [ContextMenu("Check PossibleMoves")]
        public void CheckPossibleMoves()
        {
            bool hasMoves = _boardScanService.HasPossibleMoves(GetBoard(), _boardSettings.linkMode);
            if (!hasMoves)
            {
                ShuffleBoard();
            }
        }
        public Tile[,] GetBoard() => _board;
        public ChipSettings GetChipSettings() => chipVisualConfig;
    }
}