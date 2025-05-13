using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Chip : MonoBehaviour
    {
        public ChipColor Color { get; private set; }
        public Tile ParentTile { get;  set; }
        private Color _originalColor;
        private SpriteRenderer _spriteRenderer;
        public void Initialize(ChipColor color, Tile tile)
        {
            Color = color;
            ParentTile = tile;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = GetChipColor(color);
            _spriteRenderer.color =_originalColor;
        }

        private Color GetChipColor(ChipColor color)
        {
            return color switch
            {
                ChipColor.Red => UnityEngine.Color.red,
                ChipColor.Green => UnityEngine.Color.green,
                ChipColor.Blue => UnityEngine.Color.blue,
                ChipColor.Yellow => UnityEngine.Color.yellow,
                _ => UnityEngine.Color.white
            };
        }
        public void Select()
        {
            _spriteRenderer.color = new Color(0f,0f,0f);
        }
        public void ResetColor()
        {
            _spriteRenderer.color = _originalColor;
        }

        public void DestroyChip()
        {
            Destroy(gameObject);
        }

    }
}