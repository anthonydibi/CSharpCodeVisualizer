using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ThreeSharp.Behaviours;

namespace ThreeSharp.Interaction
{
    public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color enterColor = Color.white;
        [SerializeField] private Color downColor = Color.white;
        [SerializeField] private UnityEvent OnClick = new UnityEvent();

        private MeshRenderer meshRenderer = null;
        private bool beingMoved = false;
        private bool pointerIsOnThis = false;
        private PhysicsPointer physicsPointer;
        private Collider collider;
        private UnityProjectNode unityProjectNode;
        private GameObject codeInspectorCanvas;
        private TextMeshProUGUI codeInspectorText;
        private GameObject playerEye;
        private float initialDistance;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            meshRenderer.material.color = enterColor;
            pointerIsOnThis = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            meshRenderer.material.color = normalColor;
            pointerIsOnThis = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            meshRenderer.material.color = downColor;
            initialDistance = (this.transform.position - physicsPointer.transform.position).magnitude;
            beingMoved = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            meshRenderer.material.color = enterColor;
            beingMoved = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke();
        }

        void Start()
        {
            physicsPointer = GameObject.Find("PhysicsPointer").GetComponent<PhysicsPointer>();
            collider = GetComponent<Collider>();
            unityProjectNode = GetComponentInParent<UnityProjectNode>();
            codeInspectorCanvas = GameObject.Find("CodeInspectorCanvas");
            codeInspectorText = codeInspectorCanvas.GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log(codeInspectorText.text);
            playerEye = GameObject.Find("CenterEyeAnchor");

        }

        void Update()
        {
            if (beingMoved)
            {
                gameObject.transform.parent.position = physicsPointer.transform.position + (physicsPointer.transform.forward * initialDistance);
            }
            if (pointerIsOnThis && OVRInput.Get(OVRInput.Button.One)) //"A" button on controller
            {
                codeInspectorCanvas.SetActive(true);
                codeInspectorText.text = unityProjectNode.SyntaxNode.TypeDeclaration.ToString();
                codeInspectorCanvas.transform.position = playerEye.transform.position + playerEye.transform.forward * 2;
                codeInspectorCanvas.transform.LookAt(playerEye.transform.position);
                codeInspectorCanvas.transform.Rotate(0, 180, 0);
            }
        }
    }
}
