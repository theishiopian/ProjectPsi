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

    [Header("Settings")]
    public LayerMask mask;
    public float launchForce = 20;
    public float springMultiplier = 5;
    public float raycastRadius = 0.25f;
    public float raycastRange = 100f;
    public string hitTag = "Grabbable";

    RaycastHit hit;
    private Rigidbody target;

    private void Update()
    {
        if (Physics.SphereCast(head.transform.position, raycastRadius, head.forward, out hit, raycastRange, mask))
        {
            if (hit.collider.CompareTag(hitTag))
            {
                target = hit.rigidbody;
            }
        }

        if (pickupAction.GetStateDown(controller))
        {
            if(target)
            {
                joint.connectedBody = target;
                joint.spring = target.mass * springMultiplier;
            }
        }

        if(pickupAction.GetStateUp(controller))
        {
            joint.connectedBody = null;
        }

        if(throwAction.GetStateDown(controller))
        {
            joint.connectedBody = null;
            target.AddForce(head.forward * launchForce, ForceMode.VelocityChange);
            target = null;
        }
    }
}