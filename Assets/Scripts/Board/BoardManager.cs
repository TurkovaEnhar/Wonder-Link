using System;
using DG.Tweening;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Board
{
    public class BoardManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GameObject chipPrefab;
        [SerializeField] private Sprite tileSprite;
        [Header("Settings")]
        [SerializeField] private BoardSettings boardSettings;
        [SerializeField] private ChipSettings chipVisualConfig;
        [SerializeField] private float spacing = 0.1f;
        private Tile[,] _board;

        private void Start()
        {
            GenerateBoard();
        }
        public void GenerateBoard()
        {
            int width = boardSettings.GetBoardWidth();
            int height = boardSettings.GetBoardHeight();
            _board = new Tile[width, height];
            Vector2 tileSize = GetTileSize();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 position = new Vector3(x * tileSize.x, y * tileSize.y, 0);
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
            GameObject chipGO = Instantiate(chipPrefab, spawnPos, Quaternion.identity, tile.transform);

            Chip chip = chipGO.GetComponent<Chip>();
            ChipColor randomColor = GetRandomColor();
            Sprite chipSprite = chipVisualConfig.GetSpriteForColor(randomColor);

            chip.Initialize(randomColor, tile, chipSprite);
            tile.SetChip(chip);

            chip.transform.DOMove(tile.transform.position, 0.25f).SetEase(Ease.OutBack);
        }
        public void FillBoard()
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
            chip.transform.DOMove(targetTile.transform.position, 0.25f).SetEase(Ease.Linear);
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
    }
}