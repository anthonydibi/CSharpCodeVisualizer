using CSharpCodeVisualizer.Syntax;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class GenerateProjectTreeVisualization
{
    private const float VERTICAL_DISTANCE_BETWEEN_LEVELS = 0.6f;
    private const float DISTANCE_FROM_CENTRAL_NODE = 0.5f;

    public static void GenerateNamespaceTree(Vector3 prevNodeCoordinate, ProjectNode namespaceNode, Vector3 startCoordinate, GameObject nodePrefab, Material lineMaterial)
    {
        GameObject namespaceNodeObject = InstantiateAndLabelNode(namespaceNode.Key, startCoordinate, Quaternion.identity, nodePrefab);
        Vector3 levelOffset = new Vector3(0, -VERTICAL_DISTANCE_BETWEEN_LEVELS, 0);
        if (prevNodeCoordinate != null)
        {
            LineRenderer lineToPrev = namespaceNodeObject.AddComponent<LineRenderer>();
            ConfigureLineRenderer(lineToPrev, lineMaterial);
            lineToPrev.SetPosition(0, prevNodeCoordinate);
            lineToPrev.SetPosition(1, prevNodeCoordinate + levelOffset);
            lineToPrev.SetPosition(2, startCoordinate);
        }
        for (int i = 0; i < namespaceNode.Children.Count; i++)
        {
            ProjectNode child = namespaceNode.Children[i];
            float radiansFromStart = (2 * Mathf.PI / namespaceNode.Children.Count) * i;
            Vector3 directionToNode = new Vector3(Mathf.Cos(radiansFromStart), 0, Mathf.Sin(radiansFromStart));
            Vector3 nodePosition = (startCoordinate + levelOffset) + (directionToNode * DISTANCE_FROM_CENTRAL_NODE);
            if (child is NamespaceNode)
            {
                GenerateNamespaceTree(startCoordinate, child, nodePosition, nodePrefab, lineMaterial);
            }
            else if (child is ClassNode)
            {
                GameObject classNodeObject = InstantiateAndLabelNode(child.Key, nodePosition, Quaternion.identity, nodePrefab);
                LineRenderer lineToPrev = classNodeObject.AddComponent<LineRenderer>();
                ConfigureLineRenderer(lineToPrev, lineMaterial);
                lineToPrev.SetPosition(0, startCoordinate);
                lineToPrev.SetPosition(1, startCoordinate + levelOffset);
                lineToPrev.SetPosition(2, nodePosition);
            }
        }
    }

    private static GameObject InstantiateAndLabelNode(string text, Vector3 pos, Quaternion quaternion, GameObject nodePrefab)
    {
        GameObject nodeObject = GameObject.Instantiate(nodePrefab, pos, quaternion);
        nodeObject.GetComponentInChildren<TextMeshPro>().text = text;
        return nodeObject;
    }

    private static void ConfigureLineRenderer(LineRenderer line, Material lineMaterial)
    {
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.positionCount = 3;
        line.startColor = Color.black;
        line.endColor = Color.black;
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
    }
}
