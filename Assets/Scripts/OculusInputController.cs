using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusInputController : MonoBehaviour
{
    private Vector3 lastControllerPosition;
    public GameObject rightController;
    public GameObject visualizationTransformRoot;
    // Start is called before the first frame update
    void Start()
    {
        rightController = GameObject.Find("RightHandAnchor");
        visualizationTransformRoot = GameObject.Find("VisualizationTransformRoot");
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            Vector3 visualizationTransformRootTranslation = (rightController.transform.position - lastControllerPosition) * 3;
            visualizationTransformRoot.transform.position += visualizationTransformRootTranslation;
        }
        lastControllerPosition = rightController.transform.position;
    }
}
