using System.Collections.Generic;
using Unity.Labs.SuperScience;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using cakeslice;

public class Telekinesis : MonoBehaviour
{
    [Header("SteamVR")]
    public SteamVR_Input_Sources controller;
    public SteamVR_Action_Boolean pickupAction;
    public SteamVR_Action_Boolean throwAction;

    [Header("Transforms")]
    public Transform head;

    [Header("Physics Objects")]
    public SpringJoint joint;
    public Rigidbody target;

    [Header("Settings")]
    public float launchForce = 50;

    private void Update()
    {
        if(pickupAction.GetStateDown(controller))
        {
            joint.connectedBody = target;
        }

        if(pickupAction.GetStateUp(controller))
        {
            joint.connectedBody = null;
        }

        if(throwAction.GetStateDown(controller))
        {
            joint.connectedBody = null;
            target.AddForce(head.forward * launchForce, ForceMode.VelocityChange);
        }
    }
}