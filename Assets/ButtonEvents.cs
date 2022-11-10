using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
    public OVRInput.Controller controller;
    private Vector3 previousControllerPosition;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        /*if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            transform.parent.transform.position += 1;
        }*/
    }
}
