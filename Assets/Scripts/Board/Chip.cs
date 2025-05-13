using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Chip : MonoBehaviour
    {
        public ChipColor Color { get; private set; }
        public Tile ParentTile { get;  set; }
        private SpriteRenderer _spriteRenderer;
        public void Initialize(ChipColor color, Tile tile,Sprite sprite)
        {
            Color = color;
            ParentTile = tile;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = sprite;
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