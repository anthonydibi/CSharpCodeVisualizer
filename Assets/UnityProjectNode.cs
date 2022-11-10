using CSharpCodeVisualizer.Syntax;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityProjectNode : MonoBehaviour
{
    private ProjectNode _syntaxNode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if(lineRenderer != null)
        {
            lineRenderer.SetPosition(0, this.transform.parent.transform.position);
            lineRenderer.SetPosition(1, new Vector3(this.transform.parent.transform.position.x, this.transform.position.y, this.transform.parent.transform.position.z));
            lineRenderer.SetPosition(2, this.transform.position);
        }
    }

    public void injectSyntaxNode(ProjectNode node)
    {
        this._syntaxNode = node;
    }
}
