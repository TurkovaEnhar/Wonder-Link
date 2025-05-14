using Board;
using Board.Chips;
using UnityEngine;

namespace Link
{
    public class InputHandler : MonoBehaviour
    {
        private Camera _mainCamera;
        private LinkManager _linkManager;
        private bool _isLinking;
        [SerializeField] private LinkLineDrawer lineDrawer;

        public void Initialize(LinkManager linkManager)
        {
            _linkManager = linkManager;
            _mainCamera = Camera.main;
        }

        private void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
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
#elif UNITY_IOS || UNITY_ANDROID
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                StartLink(touch.position);
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                ContinueLink(touch.position);
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                EndLink();
                break;
        }
    }
#endif
            if(_linkManager?.GetCurrentLink().Count>0)
                lineDrawer.UpdateLine(_linkManager.GetCurrentLink());
        }

        private void StartLink()
        {
            Chip chip = RaycastChip();
            if (chip)
            {
                _isLinking = true;
                _linkManager.BeginLink(chip);
            }
        }

        private void ContinueLink()
        {
            if (!_isLinking) return;

            Chip chip = RaycastChip();
            if (chip)
            {
                _linkManager.TryAddToLink(chip);
            }
        }
        private void StartLink(Vector2 pos)
        {
            Chip chip = RaycastChip(pos);
            if (chip)
            {
                _isLinking = true;
                _linkManager.BeginLink(chip);
            }
        }

        private void ContinueLink(Vector2 pos)
        {
            if (!_isLinking) return;
            Chip chip = RaycastChip(pos);
            if (chip) _linkManager.TryAddToLink(chip);
        }

        private void EndLink()
        {
            if (!_isLinking) return;

            _isLinking = false;
            _linkManager.CompleteLink();
            lineDrawer.ClearLine();
        }

        private Chip RaycastChip()
        {
            Vector2 screenPos = Input.mousePosition;
            Vector2 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);

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
        private Chip RaycastChip(Vector2 screenPos)
        {
            Vector2 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            return hit.collider?.GetComponent<Chip>();
        }
    }
}