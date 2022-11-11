using CSharpCodeVisualizer.Syntax;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using SimpleFileBrowser;

public class VisualizationGenerator : MonoBehaviour
{
    private Vector3 lastTreeStartLocation = new Vector3(0, 0, -1);
    private float verticalDistanceBetweenLevels = 0.3f;
    private float lastTreeRadius = 0;
    public GameObject nodePrefab;
    public Material lineMaterial;
    public GameObject visualizationTransformRoot;
    private Dictionary<string, GameObject> nodeMap = new Dictionary<string, GameObject>();

    public void GenerateVisualizationFromTree(ProjectTree tree)
    {
        tree.Root.Children.ForEach(node => GenerateTreeFromRoot(node));
        GenerateRelationships(tree);
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

    private void GenerateRelationships(ProjectTree tree)
    {
        Queue<ProjectNode> q = new Queue<ProjectNode>();
        q.Enqueue(tree.Root);
        while(q.Count > 0)
        {
            ProjectNode node = q.Dequeue();
            if(node is ClassNode)
            {
                GameObject baseClassNodeObject = nodeMap.GetValueOrDefault(((ClassNode)node).BaseClass);
                if (baseClassNodeObject != null)
                {
                    GameObject extendingClassNodeObject = nodeMap.GetValueOrDefault(node.Key);
                    extendingClassNodeObject.GetComponent<UnityProjectNode>().NewRelationshipLine(baseClassNodeObject);
                }
            }
            node.Children.ForEach(child => q.Enqueue(child));
        }
    }

    private void GenerateTreeFromNode(Vector3 prevNodeCoordinate, GameObject prevNode, ProjectNode projectNode, Vector3 startCoordinate)
    {
        GameObject namespaceNodeObject = InstantiateAndLabelNode(projectNode.Key, startCoordinate, Quaternion.identity);
        namespaceNodeObject.transform.SetParent(prevNode.transform, true);
        InjectSyntaxNodeIntoUnityNode(namespaceNodeObject, projectNode);
        nodeMap.Add(getNodeKey(namespaceNodeObject), namespaceNodeObject);
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
                nodeMap.Add(child.Key, classNodeObject);
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

    private string getNodeKey(GameObject node)
    {
        UnityProjectNode unityProjectNode = node.GetComponent<UnityProjectNode>();
        return unityProjectNode.SyntaxNode.Key;
    }

    private void InjectSyntaxNodeIntoUnityNode(GameObject unityNodeObject, ProjectNode projectNode)
    {
        UnityProjectNode unityProjectNode = unityNodeObject.GetComponent<UnityProjectNode>();
        unityProjectNode.InjectSyntaxNode(projectNode);
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
        tree.PopulateTreeFromProjectPath("F:/Dev/CSharpCodeVisualizer/Assets/Scripts");
        GenerateVisualizationFromTree(tree);
        FileBrowser.AskPermissions = false;
        FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
        						   () => { Debug.Log( "Canceled" ); },
        						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );
    }
}
