﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ThreeSharp.Interaction
{
    public class CanvasPointer : Pointer
    {
        public EventSystem eventSystem = null;
        public StandaloneInputModule inputModule = null;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            UpdateLength();
        }

        private void UpdateLength()
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, GetEnd());
        }

        private Vector3 GetEnd()
        {
            float distance = GetCanvasDistance();
            Vector3 endPosition = CalculateEnd(defaultLength);

            if (distance != 0.0f)
            {
                endPosition = CalculateEnd(distance);
            }

            return endPosition;
        }

        private float GetCanvasDistance()
        {
            PointerEventData eventData = new PointerEventData(eventSystem);
            eventData.position = inputModule.inputOverride.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(eventData, results);

            RaycastResult closestResult = FindFirstRaycast(results);
            float distance = closestResult.distance;

            distance = Mathf.Clamp(distance, 0.0f, defaultLength);
            return distance;
        }

        private RaycastResult FindFirstRaycast(List<RaycastResult> results)
        {
            foreach (RaycastResult result in results)
            {
                if (!result.gameObject)
                {
                    continue;
                }
                return result;
            }
            return new RaycastResult();
        }

        private Vector3 CalculateEnd(float length)
        {
            return transform.position + (transform.forward * defaultLength);
        }
    }

}