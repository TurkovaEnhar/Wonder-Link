using System.Collections.Generic;
using UnityEngine;
using Board.Chips;

namespace Link
{
    [RequireComponent(typeof(LineRenderer))]
    public class LinkLineDrawer : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
           
            _lineRenderer.positionCount = 0;
            _lineRenderer.useWorldSpace = true;
        }

        public void UpdateLine(List<Chip> chips)
        {
          
            if (chips == null || chips.Count == 0)
            {
                _lineRenderer.positionCount = 0;
                return;
            }
            if (chips.Count > 0)
            {
                Color color = chips[0].GetComponent<SpriteRenderer>().color;
                SetLineColor(color);
            }

            _lineRenderer.positionCount = chips.Count;

            for (int i = 0; i < chips.Count; i++)
            {
                _lineRenderer.SetPosition(i, chips[i].transform.position);
            }
        }

        public void ClearLine()
        {
            _lineRenderer.positionCount = 0;
        }
        public void SetLineColor(Color color)
        {
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
        }
    }
}