using Board;
using DefaultNamespace;
using UnityEngine;

namespace Link
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LinkManager linkManager;

        private bool isLinking = false;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartLink();
            }
            else if (Input.GetMouseButton(0))
            {
                ContinueLink();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                EndLink();
            }
        }

        private void StartLink()
        {
            Chip chip = RaycastChip();
            if (chip)
            {
                isLinking = true;
                linkManager.BeginLink(chip);
            }
        }

        private void ContinueLink()
        {
            if (!isLinking) return;

            Chip chip = RaycastChip();
            if (chip)
            {
                linkManager.TryAddToLink(chip);
            }
        }

        private void EndLink()
        {
            if (!isLinking) return;

            isLinking = false;
            linkManager.CompleteLink();
        }

        private Chip RaycastChip()
        {
            Vector2 screenPos = Input.mousePosition;
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(screenPos);

            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null)
            {
                var chip = hit.collider.GetComponent<Chip>();
                if (chip)
                {
                    return chip;
                }
            }

            return null;
        }
    }
}