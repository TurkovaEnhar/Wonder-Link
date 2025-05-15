using System;
using UnityEngine;

namespace Board.Chips
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Chip : MonoBehaviour
    {
        public Action<Chip> OnReturnToPool;
        public ChipColor Color { get; private set; }
        public Tile ParentTile { get;  set; }
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(ChipColor color, Tile tile,Sprite sprite)
        {
            Color = color;
            ParentTile = tile;
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
            ParentTile.ClearChip();
            OnReturnToPool?.Invoke(this);
        }

    }
}