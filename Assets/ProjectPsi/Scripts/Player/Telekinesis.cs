using System.Collections.Generic;
using Unity.Labs.SuperScience;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Telekinesis : MonoBehaviour
{
    [Header("SteamVR")]
    public SteamVR_Input_Sources controller;
    public SteamVR_Action_Boolean pickupAction;
    public SteamVR_Action_Boolean throwAction;
    public Hand handScript;

    [Header("Transforms")]
    public Transform head;
    public Transform hand;

    [Header("Physics Objects")]
    public SpringJoint joint;

    [Header("Settings")]
    public LayerMask mask;
    public float launchForce = 20;
    public float springMultiplier = 5;
    public float raycastRadius = 0.25f;
    public float raycastRange = 100f;
    public string liftTag = "Liftable";
    public string grabTag = "Item";
    public float grabForce = 25f;
    public float grabDistance = 0.15f;

    RaycastHit hit;
    private Rigidbody liftTarget;
    private Rigidbody grabTarget;

    private void Update()
    {
        Debug.LogFormat("LiftTarget is: {0}, GrabTarget is: {1}", liftTarget, grabTarget);
        if (Physics.SphereCast(head.transform.position, raycastRadius, head.forward, out hit, raycastRange, mask))
        {
            if(hit.collider.CompareTag(grabTag))
            {
                grabTarget = hit.rigidbody;
                liftTarget = null;
            }
            else if (hit.collider.CompareTag(liftTag))
            {
                liftTarget = hit.rigidbody;
                grabTarget = null;
            }
        }

        if(pickupAction.GetState(controller) && grabTarget)
        {
            grabTarget.AddForce((hand.position - grabTarget.transform.position).normalized * grabForce, ForceMode.Acceleration);

            if(Vector3.Distance(hand.position, grabTarget.transform.position) < grabDistance)
            {
                //attatch to hand
                handScript.AttachObject(grabTarget.gameObject, GrabTypes.Grip);

                grabTarget = null;
            }
        }

        if (pickupAction.GetStateDown(controller) && liftTarget)
        {
            joint.connectedBody = liftTarget;
            joint.spring = liftTarget.mass * springMultiplier;
        }

        if(pickupAction.GetStateUp(controller))
        {
            joint.connectedBody = null;
            liftTarget = null;
            grabTarget = null;
        }

        if(throwAction.GetStateDown(controller))
        {
            joint.connectedBody = null;
            liftTarget.AddForce(head.forward * launchForce, ForceMode.VelocityChange);
            liftTarget = null;
        }
    }
}