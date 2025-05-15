using Board;
using Board.Chips;
using UnityEngine;

namespace Link
{
    public class InputHandler : MonoBehaviour
    {
        private Camera _mainCamera;
        private LinkService _linkService;
        private bool _isLinking;
        [SerializeField] private LinkLineDrawer lineDrawer;

        public void Initialize(LinkService linkService)
        {
            _linkService = linkService;
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
            if(_linkService?.GetCurrentLink().Count>0)
                lineDrawer.UpdateLine(_linkService.GetCurrentLink());
        }

        private void StartLink()
        {
            Chip chip = RaycastChip();
            if (chip)
            {
                _isLinking = true;
                _linkService.BeginLink(chip);
            }
        }

        private void ContinueLink()
        {
            if (!_isLinking) return;

            Chip chip = RaycastChip();
            if (chip)
            {
                _linkService.TryAddToLink(chip);
            }
        }
        private void StartLink(Vector2 pos)
        {
            Chip chip = RaycastChip(pos);
            if (chip)
            {
                _isLinking = true;
                _linkService.BeginLink(chip);
            }
        }

        private void ContinueLink(Vector2 pos)
        {
            if (!_isLinking) return;
            Chip chip = RaycastChip(pos);
            if (chip) _linkService.TryAddToLink(chip);
        }

        private void EndLink()
        {
            if (!_isLinking) return;

            _isLinking = false;
            _linkService.CompleteLink();
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