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
    public LayerMask worldMask;
    public LayerMask pickUpMask;
    public float launchForce = 20;
    public float springMultiplier = 5;
    public float spherecastRadius = 0.25f;
    public float spherecastRange = 100f;
    public float overlapRadius = 0.5f;
    public string liftTag = "Liftable";
    public string grabTag = "Item";
    public float grabForce = 25f;
    public float grabDistance = 0.15f;

    [Header("Outline")]
    public ParticleSystem outline;

    private RaycastHit hit;
    private Rigidbody liftTarget;
    private Rigidbody grabTarget;
    private Collider[] sphereHits;


    private void Update()
    {
        Physics.SphereCast(head.transform.position, spherecastRadius, head.forward, out hit, spherecastRange, worldMask);

        if (pickupAction.GetStateDown(controller))
        {
            sphereHits = Physics.OverlapSphere(hit.point, overlapRadius, pickUpMask);

            foreach (Collider item in sphereHits)
            {
                if (item.CompareTag(grabTag))
                {
                    grabTarget = SetTarget(item.GetComponent<Rigidbody>());
                    Debug.Log("grab target");
                    //ResetTarget(out liftTarget);
                    break;
                }
                else if (item.CompareTag(liftTag))
                {
                    liftTarget = SetTarget(item.GetComponent<Rigidbody>());
                    //ResetTarget(out grabTarget);

                    joint.connectedBody = liftTarget;
                    joint.spring = liftTarget.mass * springMultiplier;
                    break;
                }
            }
        }
        else if (pickupAction.GetStateUp(controller))
        {
            joint.connectedBody = null;
            if(liftTarget)ResetTarget(out liftTarget);
            if(grabTarget)ResetTarget(out grabTarget);
        }
        else if (grabTarget && pickupAction.GetState(controller))
        {
            grabTarget.AddForce((hand.position - grabTarget.transform.position).normalized * grabForce, ForceMode.Acceleration);

            if (Vector3.Distance(hand.position, grabTarget.transform.position) < grabDistance)
            {
                //attatch to hand
                handScript.AttachObject(grabTarget.gameObject, GrabTypes.Grip);

                ResetTarget(out grabTarget);
            }
        }
        else if(throwAction.GetStateDown(controller))
        {
            joint.connectedBody = null;
            liftTarget.AddForce(head.forward * launchForce, ForceMode.VelocityChange);
            ResetTarget(out liftTarget);
            
        }

    }

    Rigidbody SetTarget(Rigidbody target)
    {
        Debug.Log("Setting");

        outline.transform.SetParent(target.transform);//this line not working, even though the method is called. fuck you
        
        outline.transform.localPosition = Vector3.zero;

        ParticleSystem.ShapeModule shape = outline.shape;

        MeshFilter filter = target.gameObject.GetComponent<MeshFilter>();

        shape.mesh = filter.mesh;

        outline.Play();

        return target;
    }

    void ResetTarget(out Rigidbody target)
    {
        outline.Stop();
        outline.transform.parent = null;
        outline.transform.position = Vector3.zero;
        Debug.Log("Resetting");
        target = null;
    }
}