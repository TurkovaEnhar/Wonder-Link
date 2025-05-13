using System.Collections.Generic;
using Board;
using Game;
using Link;
using UnityEngine;

namespace Test
{
    public class JamTheBoard : MonoBehaviour
    {
        [SerializeField] private BoardManager boardManager;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private BoardAnalyzer analyzer;

        private static readonly ChipColor[,] JammedGrid8x8 = new ChipColor[8, 8]
        {
            {
                ChipColor.Blue, ChipColor.Blue, ChipColor.Red, ChipColor.Red, ChipColor.Blue, ChipColor.Blue,
                ChipColor.Red, ChipColor.Red
            },
            {
                ChipColor.Green, ChipColor.Green, ChipColor.Yellow, ChipColor.Yellow, ChipColor.Green, ChipColor.Green,
                ChipColor.Yellow, ChipColor.Yellow
            },
            {
                ChipColor.Blue, ChipColor.Blue, ChipColor.Red, ChipColor.Red, ChipColor.Blue, ChipColor.Blue,
                ChipColor.Red, ChipColor.Red
            },
            {
                ChipColor.Green, ChipColor.Green, ChipColor.Yellow, ChipColor.Yellow, ChipColor.Green, ChipColor.Green,
                ChipColor.Yellow, ChipColor.Yellow
            },
            {
                ChipColor.Blue, ChipColor.Blue, ChipColor.Red, ChipColor.Red, ChipColor.Blue, ChipColor.Blue,
                ChipColor.Red, ChipColor.Red
            },
            {
                ChipColor.Green, ChipColor.Green, ChipColor.Yellow, ChipColor.Yellow, ChipColor.Green, ChipColor.Green,
                ChipColor.Yellow, ChipColor.Yellow
            },
            {
                ChipColor.Blue, ChipColor.Blue, ChipColor.Red, ChipColor.Red, ChipColor.Blue, ChipColor.Blue,
                ChipColor.Red, ChipColor.Red
            },
            {
                ChipColor.Green, ChipColor.Green, ChipColor.Yellow, ChipColor.Yellow, ChipColor.Green, ChipColor.Green,
                ChipColor.Yellow, ChipColor.Yellow
            },
        };

        [ContextMenu("8 way Jam")]
        private void ForceNoMoves_Guaranteed()
        {
            Tile[,] board = boardManager.GetBoard();
            int width = board.GetLength(0);
            int height = board.GetLength(1);

            if (width != 8 || height != 8)
            {
                Debug.LogError("Jammed grid is hardcoded as 8x8!");
                return;
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = board[x, y];
                    Chip chip = tile.CurrentChip;
                    if (chip == null) continue;

                    ChipColor color = JammedGrid8x8[x, y];
                    Sprite sprite = boardManager.GetChipSettings().GetSpriteForColor(color);
                    chip.Initialize(color, tile, sprite);
                    tile.SetChip(chip);
                    chip.transform.position = tile.transform.position;
                }
            }

            Debug.Log("✅ Sabit jammed board uygulandı.");
        }

        [ContextMenu("4 way jam")]
        private void ForceNoMoves()
        {
            Tile[,] board = boardManager.GetBoard();
            int width = board.GetLength(0);
            int height = board.GetLength(1);

            ChipColor[] safePattern =
            {
                ChipColor.Red, ChipColor.Green, ChipColor.Blue
            };

            // Shift pattern her satırda/kolonda dönecek
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = board[x, y];
                    Chip chip = tile.CurrentChip;

                    if (chip == null) continue;

                    // Düzgün karışık ama eşleşme yaratmayan renk seçimi
                    int index = (x + y * 2) % safePattern.Length; // Deseni kaydır
                    ChipColor color = safePattern[index];
                    Sprite sprite = boardManager.GetChipSettings().GetSpriteForColor(color);

                    chip.Initialize(color, tile, sprite);
                    tile.SetChip(chip);
                    chip.transform.position = tile.transform.position;
                }
            }

            Debug.Log($"✅ Board forcibly jammed for {gameConfig.linkMode} mode.");
        }
    }
}