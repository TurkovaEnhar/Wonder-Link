using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Chip : MonoBehaviour
    {
        public ChipColor Color { get; private set; }
        public Tile ParentTile { get;  set; }
        private Sprite _originalSprite;
        private SpriteRenderer _spriteRenderer;
        public void Initialize(ChipColor color, Tile tile,Sprite sprite)
        {
            Color = color;
            ParentTile = tile;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = sprite;
            _originalSprite = sprite;
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
            _spriteRenderer.color = UnityEngine.Color.black;
        }
        public void ResetColor()
        {
            _spriteRenderer.color = UnityEngine.Color.white; 
        }

        public void DestroyChip()
        {
            Destroy(gameObject);
        }

    }
}