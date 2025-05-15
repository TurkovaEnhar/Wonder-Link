using TMPro;
using UnityEngine;

namespace MoveSystem
{
    public class MoveView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moveText;

        public void UpdateMoveText(int remaining)
        {
            moveText.text = $"Move: {remaining}";
        }
    }
}