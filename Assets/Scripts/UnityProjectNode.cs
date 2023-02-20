using ThreeSharp.Parsing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThreeSharp.Behaviours
{
    public class UnityProjectNode : MonoBehaviour
    {
        private ProjectNode _syntaxNode;
        public Material lineMaterial;
        private List<LineRenderer> _attachedLines = new List<LineRenderer>();
        private List<LineRenderer> _originatingLines = new List<LineRenderer>();

        public ProjectNode SyntaxNode
        {
            get
            {
                return _syntaxNode;
            }
        }
        // Start is called before the first frame update
        private void Start()
        {
            if (this._syntaxNode is ClassNode)
            {
                this.transform.localScale *= 1 + (((ClassNode)this._syntaxNode).TypeDeclaration.Members.Count / (float)ProjectConstants.maxClassMembers);  //scale node depending on how many members it has (smaller nodes have less members)
            }
        }

        // Update is called once per frame
        void Update()
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, this.transform.parent.transform.position);
                lineRenderer.SetPosition(1, new Vector3(this.transform.parent.transform.position.x, this.transform.position.y, this.transform.parent.transform.position.z));
                lineRenderer.SetPosition(2, this.transform.position);
            }
            _originatingLines.ForEach(line => SetArrowPositions(line, line.GetPosition(3), this.transform.position));
            _attachedLines.ForEach(line => SetArrowPositions(line, this.transform.position, line.GetPosition(3)));
        }

        public void InjectSyntaxNode(ProjectNode node)
        {
            this._syntaxNode = node;
        }

        public void AttachLine(LineRenderer line)
        {
            _attachedLines.Add(line);
        }

        public void NewRelationshipLine(GameObject nodeToDrawTo)
        {
            GameObject lineRendererHolder = new GameObject("LineRendererHolder");
            lineRendererHolder.transform.parent = this.transform;
            lineRendererHolder.AddComponent<LineRenderer>();
            LineRenderer line = lineRendererHolder.GetComponent<LineRenderer>();
            line.material = lineMaterial;
            line.positionCount = 4;
            line.startColor = Color.black;
            line.endColor = Color.black;
            float percentHead = 0.2f;
            line.widthCurve = new AnimationCurve(
                 new Keyframe(0, 0.01f)
                 , new Keyframe(0.999f - percentHead, 0.01f)  // neck of arrow
                 , new Keyframe(1 - percentHead, 0.04f)  // max width of arrow head
                 , new Keyframe(1, 0f));  // tip of arrow
            SetArrowPositions(line, nodeToDrawTo.transform.position, this.transform.position);
            nodeToDrawTo.GetComponent<UnityProjectNode>().AttachLine(line);
            _originatingLines.Add(line);
        }

        private void SetArrowPositions(LineRenderer line, Vector3 origin, Vector3 target)
        {
            line.SetPositions(new Vector3[] {
              origin
              , Vector3.Lerp(origin, target, 0.999f - .2f)
              , Vector3.Lerp(origin, target, 1 - .2f)
              , target });
        }
    }
}