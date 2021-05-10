using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Telekinesis : MonoBehaviour
{
    #region header
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
    public GameObject particles;
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
    public float sphereRadius = 0.5f;
    public float castDistance = 100;
    public LayerMask rayMask;
    public LayerMask sphereMask;
    public string ignoreLayer = "Items";
    #endregion

    //internal vars
    private RaycastHit hit;
    private Rigidbody liftTarget;
    private Rigidbody grabTarget;
    private bool lifting  = false;
    private bool grabbing = false;
    private Collider[] overlaps;

    private void Start()
    {
        if(!outline)
        {
            RemakeParticles();
        }
    }

    void RemakeParticles()
    {
        outline = Instantiate(particles).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        #region OldCode
        //if (!lifting && handScript.AttachedObjects.Count == 0)
        //{
        //    //scan for targets
        //    if (Physics.SphereCast(look.position, castRadius, look.forward, out hit, castDistance, castMask))
        //    {
        //        overlaps = Physics.OverlapSphere(hit.point, sphereRadius, sphereMask);
        //        float distance = Mathf.Infinity;
        //        Collider theOne = null;

        //        //sort by size
        //        foreach (Collider c in overlaps)
        //        {
        //            float newDist = Vector3.Distance(hit.point, c.transform.position);
        //            if (newDist < distance && (c.CompareTag(liftTag) || c.CompareTag(grabTag)))
        //            {
        //                if(c.gameObject.layer == LayerMask.NameToLayer(ignoreLayer) && c.GetComponent<Item>().isHeld)
        //                {
        //                    continue;
        //                }
        //                distance = newDist;
        //                theOne = c;
        //            }
        //        }

        //        //aquire target
        //        if (theOne)
        //        {
        //            SetOutline(theOne.gameObject);

        //            if (theOne.CompareTag(grabTag))
        //            {
        //                grabTarget = theOne.GetComponent<Rigidbody>();
        //            }
        //            else if (theOne.CompareTag(liftTag) && !grabbing)
        //            {
        //                liftTarget = theOne.GetComponent<Rigidbody>();
        //            }
        //        }
        //        else
        //        {
        //            //if no targets to aquire at hit
        //            liftTarget = null;
        //            grabTarget = null;
        //            ResetOutline();
        //        }
        //    }
        //    else
        //    {
        //        //if no hits
        //        liftTarget = null;
        //        grabTarget = null;
        //        ResetOutline();
        //    }
        //}
        #endregion

        if(!outline)
        {
            RemakeParticles();//only calls once
        }

        if(!lifting)
        {
            float dist = castDistance;

            if (Physics.Raycast(head.position, head.forward, out hit, castDistance, rayMask))
            {
                dist = hit.distance - sphereRadius;
            }

            RaycastHit[] potentialTargets = Physics.SphereCastAll(head.position, sphereRadius, head.forward, dist, sphereMask);

            float highestMass = 0;
            Rigidbody theOne = null;

            foreach (RaycastHit canidate in potentialTargets)
            {
                //prevent us from targeting held items
                Item item = canidate.collider.transform.root.gameObject.GetComponent<Item>();
                int layer = canidate.collider.gameObject.layer;

                if (layer == LayerMask.NameToLayer(ignoreLayer) && item.isHeld)
                {
                    continue;
                }
                else
                {
                    theOne = canidate.collider.gameObject.GetComponent<Rigidbody>();

                    if (theOne && theOne.mass > highestMass)
                    {
                        highestMass = theOne.mass;
                    }
                }
            }

            if (highestMass > 0 && theOne)
            {
                SetOutline(theOne.gameObject);

                if (theOne.CompareTag(grabTag))
                {
                    grabTarget = theOne;
                }
                else if (theOne.CompareTag(liftTag) && !grabbing)
                {
                    liftTarget = theOne;
                }
            }
            else
            {
                //if no targets to aquire at hit
                liftTarget = null;
                grabTarget = null;
                ResetOutline();
            }
        }

        if (pickupAction.GetStateUp(controller))//let go
        {
            lifting = false;
            joint.connectedBody = null;
        }
        else if (pickupAction.GetState(controller))//use telekinesis
        {
            if(grabTarget)//item
            {
                lifting = true;
                grabbing = true;
                grabTarget.AddForce((hand.position - grabTarget.transform.position).normalized * grabForce, ForceMode.Acceleration);

                if (Vector3.Distance(hand.position, grabTarget.transform.position) < grabDistance)//is close enough to hand
                {
                    //attatch to hand
                    lifting = false;
                    grabbing = false;
                    handScript.AttachObject(grabTarget.gameObject, GrabTypes.Grip);

                    grabTarget = null;
                    ResetOutline();
                }  
            }
            else if (liftTarget && !grabbing)//object
            {
                lifting = true;

                joint.connectedBody = liftTarget;
                joint.spring = liftTarget.mass * springMultiplier;
            }
        }

        if(throwAction.GetStateDown(controller))//throw
        {
            if(liftTarget)
            {
                joint.connectedBody = null;
                liftTarget.AddForce(head.forward * launchForce, ForceMode.VelocityChange);
                liftTarget = null;
                lifting = false;
                ResetOutline();
            }
        }
    }

    void SetOutline(GameObject target)//move particles to target
    {
        ParticleSystem.ShapeModule shape = outline.shape;

        //MeshFilter filter = target.GetComponent<MeshFilter>();

        //shape.mesh = filter.mesh;

        outline.transform.SetParent(target.transform);
        outline.transform.localPosition = Vector3.zero;
        outline.transform.localEulerAngles = Vector3.zero;
        outline.Play();
    }

    void ResetOutline()//stop particles
    {
        outline.Stop();
    }
}