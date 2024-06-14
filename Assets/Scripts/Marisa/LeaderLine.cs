using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marisa
{
    public class LeaderLine : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public Transform target;
        public RectTransform leaderLineNameText;
        public Canvas canvas;
        public CanvasGroup canvasGroup;
        public bool isHide = false;

        public void LateUpdate()
        {
            UpdateLineRenderer();
            Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(target.position));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject != target.gameObject)
                    Hide();
                else
                    Show();
            }

        }

        private void UpdateLineRenderer()
        {
            if (isHide) return;

            var viewPos = Camera.main.WorldToViewportPoint(target.position);
            if (viewPos.x >= 0 && viewPos.x <= 1 &&
                viewPos.y >= 0 && viewPos.y <= 1 &&
                viewPos.z >= 0)
            {
                var targetPos = CalculateTargetScreenPoint();
                var uiPos = GetAnchoredPos();
                var offset = new Vector2(0, 0);

                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, targetPos);
                lineRenderer.SetPosition(1, uiPos + offset);
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }

        public void Hide()
        {
            if (canvasGroup.alpha == 0f)
                return;
            DOTween.Kill("Fading_" + transform.GetInstanceID());
            canvasGroup.DOFade(0f, canvasGroup.alpha * 0.2f).OnComplete(() =>
            {
                lineRenderer.enabled = false; 
                isHide = true; 
            })
                .SetId("Fading_"+transform.GetInstanceID());
        }

        public void Show()
        {
            if (canvasGroup.alpha == 1f)
                return;
            DOTween.Kill("Fading_" + transform.GetInstanceID());
            canvasGroup.DOFade(1f, (1f - canvasGroup.alpha) * 0.2f).OnComplete(() => 
            {
                lineRenderer.enabled = true; 
                isHide = false; 
            })
                .SetId("Fading_" + transform.GetInstanceID());
        }

        private static Vector2 HALF_SCREEN = new Vector2(-960f,-540f);

        public void UpdateNameTextPosition()
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(target.position);
            leaderLineNameText.anchoredPosition = screenPoint + HALF_SCREEN ;
        }

        public Vector2 CalculateTargetScreenPoint()
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(target.position);
            return screenPoint + HALF_SCREEN;
        }


        public Vector2 GetAnchoredPos()
        {
            return leaderLineNameText.anchoredPosition;
        }

        public void SetAnchoredPos(Vector2 anchoredPosition)
        {
            leaderLineNameText.anchoredPosition = anchoredPosition;
        }

    }
}