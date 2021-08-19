using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;


namespace ZenvaVR
{
    [RequireComponent(typeof(VRInteractiveItem))]
    public class Draggable : MonoBehaviour
    {
        public float cameraDistance = 1;

        //max distance grabbing
        public float maxGrabDistance = 2;

        //VR interactive item 
        VRInteractiveItem vrInteractive;
        // Start is called before the first frame update


        enum State {Ready, Dragging, Blocked }

        //Current state 
        State currState;

        // inital position 
        Vector3 initialPos;

        void Awake()
        {
            vrInteractive = GetComponent<VRInteractiveItem>();

            currState = State.Ready; 
        }
    }
}

