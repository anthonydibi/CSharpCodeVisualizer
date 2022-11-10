using CSharpCodeVisualizer.Syntax;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualizationGenerator : MonoBehaviour
{
    private Vector3 lastTreeStartLocation = new Vector3(0, 0, -1);
    private float verticalDistanceBetweenLevels = 0.3f;
    private float lastTreeRadius = 0;
    public GameObject nodePrefab;
    public Material lineMaterial;
    public GameObject visualizationTransformRoot;

    public void GenerateVisualizationFromTree(ProjectTree tree)
    {
        tree.Root.Children.ForEach(node => GenerateTreeFromRoot(node));
    }

    private void GenerateTreeFromRoot(ProjectNode rootNode)
    {
        verticalDistanceBetweenLevels = ProjectConstants.VERTICAL_DISTANCE_BETWEEN_LEVELS;
        float thisTreeRadius = (float)rootNode.Height * ProjectConstants.DISTANCE_FROM_CENTRAL_NODE + 0.1f;  //approximation of the tree radius, not accurate for trees that are unbalanced - also leaves some space in between
        Vector3 nextTreeDirection = Vector3.right;
        Vector3 thisTreeStartLocation = lastTreeStartLocation + (nextTreeDirection * lastTreeRadius) + (nextTreeDirection * thisTreeRadius);
        GenerateTreeFromNode(thisTreeStartLocation, visualizationTransformRoot, rootNode, thisTreeStartLocation);
        lastTreeStartLocation = thisTreeStartLocation;
        lastTreeRadius = thisTreeRadius;
    }

    private void GenerateTreeFromNode(Vector3 prevNodeCoordinate, GameObject prevNode, ProjectNode projectNode, Vector3 startCoordinate)
    {
        GameObject namespaceNodeObject = InstantiateAndLabelNode(projectNode.Key, startCoordinate, Quaternion.identity);
        namespaceNodeObject.transform.SetParent(prevNode.transform, true);
        InjectSyntaxNodeIntoUnityNode(namespaceNodeObject, projectNode);
        UnityProjectNode unityNamespaceNode = namespaceNodeObject.GetComponentInChildren<UnityProjectNode>();
        unityNamespaceNode.injectSyntaxNode(projectNode);
        Vector3 levelOffset = new Vector3(0, -verticalDistanceBetweenLevels, 0);
        if (prevNodeCoordinate != startCoordinate)
        {
            LineRenderer lineToPrev = namespaceNodeObject.AddComponent<LineRenderer>();
            ConfigureLineRenderer(lineToPrev);
            lineToPrev.SetPosition(0, prevNodeCoordinate);
            lineToPrev.SetPosition(1, prevNodeCoordinate + levelOffset);
            lineToPrev.SetPosition(2, startCoordinate);
        }
        for (int i = 0; i < projectNode.Children.Count; i++)
        {
            ProjectNode child = projectNode.Children[i];
            float radiansFromStart = (2 * Mathf.PI / projectNode.Children.Count) * i;
            Vector3 directionToNode = new Vector3(Mathf.Cos(radiansFromStart), 0, Mathf.Sin(radiansFromStart));
            Vector3 nodePosition = (startCoordinate + levelOffset) + (directionToNode * ProjectConstants.DISTANCE_FROM_CENTRAL_NODE);
            if (child is NamespaceNode)
            {
                verticalDistanceBetweenLevels += .07f;
                GenerateTreeFromNode(startCoordinate, namespaceNodeObject, child, nodePosition);
            }
            else if (child is ClassNode)
            {
                GameObject classNodeObject = InstantiateAndLabelNode(child.Key, nodePosition, Quaternion.identity);
                classNodeObject.transform.SetParent(namespaceNodeObject.transform, true);
                InjectSyntaxNodeIntoUnityNode(classNodeObject, child);
                LineRenderer lineToPrev = classNodeObject.AddComponent<LineRenderer>();
                ConfigureLineRenderer(lineToPrev);
                lineToPrev.SetPosition(0, startCoordinate);
                lineToPrev.SetPosition(1, startCoordinate + levelOffset);
                lineToPrev.SetPosition(2, nodePosition);
            }
        }
    }

    private GameObject InstantiateAndLabelNode(string text, Vector3 pos, Quaternion quaternion)
    {
        GameObject nodeObject = Instantiate(nodePrefab, pos, quaternion);
        nodeObject.GetComponentInChildren<TextMeshPro>().text = text;
        return nodeObject;
    }

    private void InjectSyntaxNodeIntoUnityNode(GameObject unityNodeObject, ProjectNode projectNode)
    {
        UnityProjectNode unityProjectNode = unityNodeObject.GetComponentInChildren<UnityProjectNode>();
        unityProjectNode.injectSyntaxNode(projectNode);
    }

    private void ConfigureLineRenderer(LineRenderer line)
    {
        line.material = lineMaterial;
        line.positionCount = 3;
        line.startColor = Color.black;
        line.endColor = Color.black;
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
    }

    void Start()
    {
        visualizationTransformRoot = GameObject.Find("VisualizationTransformRoot");
        ProjectTree tree = new ProjectTree();
        tree.PopulateTreeFromProjectPath("F:/Dev/CSharpCodeVisualizer");
        GenerateVisualizationFromTree(tree);
    }
}
