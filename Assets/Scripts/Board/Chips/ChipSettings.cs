using UnityEngine;

namespace Board.Chips
{
    [System.Serializable]
    public class ChipSpriteData
    {
        public ChipColor color;
        public Sprite sprite;
    }
    [CreateAssetMenu(menuName = "Game/New Chip")]
    public class ChipSettings : ScriptableObject
    {
        public ChipSpriteData[] chipSprites;

        public Sprite GetSpriteForColor(ChipColor color)
        {
            foreach (var data in chipSprites)
            {
                if (data.color == color)
                    return data.sprite;
            }
            
            return null;
        }
    }
    
}