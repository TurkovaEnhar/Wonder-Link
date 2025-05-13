using System;
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

            GameObject chipGO = Instantiate(chipPrefab, tile.transform.position, Quaternion.identity, tile.transform);
            Chip chip = chipGO.GetComponent<Chip>();

            ChipColor randomColor = GetRandomColor();
            chip.Initialize(randomColor, tile);

            tile.SetChip(chip);
        }
        private Vector2 GetTileSize()
        {
            if (tileSprite == null)
            {
                Debug.LogWarning("Tile Sprite not assigned! Using default size.");
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