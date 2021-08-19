using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

namespace ZenvaVR
{
    [RequireComponent(typeof(VRInteractiveItem))]
    public class Draggable : MonoBehaviour {

        // Distance from the camera when dragging
        public float cameraDistance = 3;

        // Max distance for grabbing
        public float maxGrabDistance = 4;

        // Event for when dragging starts
        public event Action OnDrag;

        // Event for when dropping stops
        public event Action OnDrop;

        // VR interactive item component
        VRInteractiveItem vrInteractive;

        // states
        enum State { Ready, Dragging, Blocked }

        // current state
        State currState;

        // Initial position
        Vector3 initialPos;

        // Initial rotation
        Quaternion initialRot;

        // Keep track of whether something is being dragged
        static bool isDragging = false;

        void Awake()
        {
            // grab our vr interactive item component
            vrInteractive = GetComponent<VRInteractiveItem>();

            // start as Ready
            currState = State.Ready;

            // save initial position
            initialPos = transform.position;

            // save initial rotation
            initialRot = transform.rotation;
        }

        void OnEnable()
        {
            vrInteractive.OnClick += HandleClick;
        }

        void OnDisable()
        {
            vrInteractive.OnClick -= HandleClick;
        }



        void HandleClick()
        {
            // if it's Ready and we are not dragging something else, we start dragging
            if (currState == State.Ready && !Draggable.isDragging)
            {
                // Check that it's not too far
                float dist = Vector3.Distance(transform.position, Camera.main.transform.position);

                if (dist > maxGrabDistance) return;

                // Set the state to Dragging
                currState = State.Dragging;

                // Set the flag to true
                Draggable.isDragging = true;

                // Execute event
                if (OnDrag != null)
                    OnDrag();
            }
            // if it's Dragging, then we will release
            else if(currState == State.Dragging)
            {
                // Set the state to Ready
                currState = State.Ready;

                // Set the flag to true
                Draggable.isDragging = false;

                // Execute event
                if (OnDrop != null)
                    OnDrop();
            }

        }

        void Update()
        {
            // check that we are Dragging
            if (currState == State.Dragging)
            {
                // grab camera
                Transform camTransf = Camera.main.transform;

                // set the position at cameraDistance
                transform.position = camTransf.position + camTransf.forward * cameraDistance;

                // make it face us
                transform.LookAt(camTransf.position);
            }
        }

        // send the item back to it it's original pos and rot
        public void SendToOriginalPos()
        {
            transform.position = initialPos;
            transform.rotation = initialRot;
        }

        // block a draggable item
        public void ToggleBlock(bool isBlock)
        {
            if(isBlock)
            {
                currState = State.Blocked;
            }
            else
            {
                currState = State.Ready;
            }
        }
    }
}
