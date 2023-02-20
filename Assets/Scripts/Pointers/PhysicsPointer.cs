using UnityEngine;

namespace ThreeSharp.Interaction
{
    public class PhysicsPointer : Pointer
    {

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
            lineRenderer.SetPosition(1, CalculateEnd());
        }

        public Vector3 CalculateEnd()
        {
            RaycastHit hit = CreateForwardRaycast();
            Vector3 endPosition = DefaultEnd(defaultLength);

            if (hit.collider)
            {
                endPosition = hit.point;
            }
            return endPosition;
        }

        private RaycastHit CreateForwardRaycast()
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);

            Physics.Raycast(ray, out hit, defaultLength);
            return hit;
        }

        private Vector3 DefaultEnd(float length)
        {
            return transform.position + (transform.forward * defaultLength);
        }
    }

}