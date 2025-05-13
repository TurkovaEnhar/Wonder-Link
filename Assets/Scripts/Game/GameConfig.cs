using Link;

namespace Game
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game/Game Config", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public LinkMode linkMode = LinkMode.FourWay;
    }

}