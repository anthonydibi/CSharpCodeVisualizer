using CSharpCodeVisualizer.Syntax;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testtttt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nodePrefab;
    public Material lineMaterial;
    // Start is called before the first frame update
    void Start()
    {
        ProjectTree tree = new ProjectTree();
        tree.PopulateTreeFromProjectPath("C:/Users/ajdib/RiderProjects/CodeVisualizerProjectParser/CodeVisualizerProjectParser");
        GenerateProjectTreeVisualization.GenerateNamespaceTree(new Vector3(0, 1, -0.5f), tree.Root.Children[0], new Vector3(0, 0.5f, -0.5f), nodePrefab, lineMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
