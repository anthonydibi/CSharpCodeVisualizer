﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ThreeSharp.Interaction
{
    public class VRInput : BaseInput
    {
        public Camera eventCamera = null;

        public OVRInput.Button clickButton = OVRInput.Button.PrimaryIndexTrigger;
        public OVRInput.Button gripButton = OVRInput.Button.PrimaryHandTrigger;
        public OVRInput.Controller controller = OVRInput.Controller.All;

        protected override void Awake()
        {
            GetComponent<BaseInputModule>().inputOverride = this;
        }

        public override bool GetButtonDown(string buttonName)
        {
            return OVRInput.GetDown(gripButton, controller);
        }

        /// <summary>
        /// Interface to Input.GetMouseButton. Can be overridden to provide custom input instead of using the Input class.
        /// </summary>

        public override bool GetMouseButton(int button)
        {
            return OVRInput.Get(clickButton, controller);
        }

        public override bool GetMouseButtonDown(int button)
        {
            return OVRInput.GetDown(clickButton, controller);
        }

        public override bool GetMouseButtonUp(int button)
        {
            return OVRInput.GetUp(clickButton, controller);
        }
        public override Vector2 mousePosition
        {
            get
            {
                return new Vector2(eventCamera.pixelWidth / 2, eventCamera.pixelHeight / 2);
            }
        }
    }
}

