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
    public Transform look;

    [Header("Objects")]
    public ParticleSystem outline;
    public SpringJoint joint;

    [Header("Physics Settings")]
    public float launchForce = 20;
    public float springMultiplier = 5;
    public string liftTag = "Liftable";
    public string grabTag = "Item";
    public float grabForce = 25f;
    public float grabDistance = 0.15f;

    [Header("Targeting Settings")]
    public float castRadius = 0.25f;
    public float sphereRadius = 0.5f;
    public float castDistance = 150;
    public LayerMask castMask;
    public LayerMask sphereMask;

    private RaycastHit hit;
    private Rigidbody liftTarget;
    private Rigidbody grabTarget;
    private bool lifting  = false;
    private Collider[] overlaps;

    private void Update()
    {
        if (!lifting && handScript.AttachedObjects.Count == 0)
        {
            if (Physics.SphereCast(look.position, castRadius, look.forward, out hit, castDistance, castMask))
            {
                overlaps = Physics.OverlapSphere(hit.point, sphereRadius, sphereMask);
                float distance = Mathf.Infinity;
                Collider theOne = null;

                foreach (Collider c in overlaps)
                {
                    float newDist = Vector3.Distance(hit.point, c.transform.position);
                    if (newDist < distance && (c.CompareTag(liftTag) || c.CompareTag(grabTag)))
                    {
                        distance = newDist;
                        theOne = c;
                    }
                }

                if (theOne)
                {
                    SetOutline(theOne.gameObject);

                    if (theOne.CompareTag(grabTag))
                    {
                        grabTarget = theOne.GetComponent<Rigidbody>();
                    }
                    else if (theOne.CompareTag(liftTag))
                    {
                        liftTarget = theOne.GetComponent<Rigidbody>();
                    }
                }
                else
                {
                    liftTarget = null;
                    grabTarget = null;
                    ResetOutline();
                }
            }
            else
            {

                liftTarget = null;
                grabTarget = null;
                ResetOutline();
            }
        }

        //Debug.Log(liftTarget);

        if (pickupAction.GetStateUp(controller))
        {
            lifting = false;
            joint.connectedBody = null;
        }
        else if (grabTarget && pickupAction.GetState(controller) )
        {
            lifting = true;
            grabTarget.AddForce((hand.position - grabTarget.transform.position).normalized * grabForce, ForceMode.Acceleration);

            if (Vector3.Distance(hand.position, grabTarget.transform.position) < grabDistance)
            {
                //attatch to hand
                lifting = false;
                handScript.AttachObject(grabTarget.gameObject, GrabTypes.Grip);

                grabTarget = null;
                ResetOutline();
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
            liftTarget = null;
            lifting = false;
            ResetOutline();
        }

    }

    void SetOutline(GameObject target)
    {
        ParticleSystem.ShapeModule shape = outline.shape;

        MeshFilter filter = target.GetComponent<MeshFilter>();

        shape.mesh = filter.mesh;

        outline.transform.SetParent(target.transform);
        outline.transform.localPosition = Vector3.zero;
        outline.transform.localEulerAngles = Vector3.zero;
        outline.Play();
    }

    void ResetOutline()
    {
        outline.Stop();
    }

}