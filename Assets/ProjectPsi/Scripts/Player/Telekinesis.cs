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
    //public LayerMask worldMask;
    //public LayerMask pickUpMask;
    public float launchForce = 20;
    public float springMultiplier = 5;
    //public float spherecastRadius = 0.25f;
    //public float spherecastRange = 100f;
    //public float overlapRadius = 0.5f;
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
    private bool lifting  = false;

    private void Update()
    {
        if(!lifting)
        {
            GameObject check = TargetScanner.Target;

            if (check)
            {
                if (check.CompareTag(grabTag))
                {
                    grabTarget = SetTarget(check.GetComponent<Rigidbody>());
                }
                else if (check.CompareTag(liftTag))
                {
                    liftTarget = SetTarget(check.GetComponent<Rigidbody>());
                }
                else
                {
                    ResetAll();
                }
            }
            else
            {
                ResetAll();
            }

            //Debug.Log(check);
        }

        //Debug.Log(liftTarget);

        if (pickupAction.GetStateUp(controller))
        {
            lifting = false;
            joint.connectedBody = null;
        }
        else if (grabTarget && pickupAction.GetState(controller))
        {
            lifting = true;
            grabTarget.AddForce((hand.position - grabTarget.transform.position).normalized * grabForce, ForceMode.Acceleration);

            if (Vector3.Distance(hand.position, grabTarget.transform.position) < grabDistance)
            {
                //attatch to hand
                handScript.AttachObject(grabTarget.gameObject, GrabTypes.Grip);

                ResetTarget(out grabTarget);
                //lifting = false;
            }
        }
        else if(liftTarget && pickupAction.GetState(controller))
        {
            lifting = true;

            joint.connectedBody = liftTarget;
            joint.spring = liftTarget.mass * springMultiplier;
        }
        else if(throwAction.GetStateDown(controller))
        {
            joint.connectedBody = null;
            liftTarget.AddForce(head.forward * launchForce, ForceMode.VelocityChange);
            ResetTarget(out liftTarget);
            lifting = false;
        }

    }

    Rigidbody SetTarget(Rigidbody target)
    {
        outline.transform.SetParent(target.transform);//this line not working, even though the method is called. fuck you
        
        outline.transform.localPosition = Vector3.zero;

        ParticleSystem.ShapeModule shape = outline.shape;

        MeshFilter filter = target.gameObject.GetComponent<MeshFilter>();

        shape.mesh = filter.mesh;

        outline.Play();

        return target;
    }

    void ResetAll()
    {
        ResetTarget(out liftTarget);
        ResetTarget(out grabTarget);
        Debug.Log("reseting");
    }

    void ResetTarget(out Rigidbody target)
    {
        outline.Stop();
        outline.transform.parent = null;
        outline.transform.position = Vector3.zero;
        target = null;
    }
}