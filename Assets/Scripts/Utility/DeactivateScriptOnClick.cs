using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Extensions
{
    public class DeactivateScriptOnClick : MonoBehaviour, IPointerClickHandler
    {
        public Action OnClick;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsTopmostUI(eventData))
                return;
            OnClick?.Invoke();
            gameObject.SetActive(false);
        }
        private bool IsTopmostUI(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            // If any UI element is above this one in the raycast result list, cancel
            foreach (var result in results)
            {
                if (result.gameObject == gameObject)
                    return true;

                // If another object was hit before this one, it's on top
                if (result.gameObject != gameObject)
                    return false;
            }

            return false; // Default to false if not found
        }
    }
}