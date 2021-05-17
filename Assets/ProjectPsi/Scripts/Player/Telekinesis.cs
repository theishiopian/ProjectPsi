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
    public Hand.AttachmentFlags attachmentFlags;

    [Header("Transforms")]
    public Transform head;
    public Transform hand;
    public Transform look;

    [Header("Objects")]
    public GameObject particles;
    public ParticleSystem outline;
    public SpringJoint joint;
    public AudioSource soundLoop;

    [Header("Physics Settings")]
    public float launchForce = 20;
    public float springMultiplier = 5;
    public string[] liftTags = {"Liftable"};
    public string[] grabTags = {"Item", "Marker", "Eraser"};
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
    private bool stopSound = false;

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
        if(!outline)
        {
            RemakeParticles();//only calls once
        }

        if(!IsHolding()) Targeting();

        if (pickupAction.GetStateUp(controller))//let go
        {
            LetGo();
        }
        else if (pickupAction.GetState(controller))//use telekinesis
        {
            Grab();
        }

        if (throwAction.GetStateDown(controller))//throw
        {
            if (liftTarget)
            {
                joint.connectedBody = null;
                liftTarget.AddForce(head.forward * launchForce, ForceMode.VelocityChange);
                OneshotManager.instance.PlaySound("psi_throw", transform.position);
                liftTarget = null;
                lifting = false;
                ResetOutline();
            }
        }

        if (stopSound && soundLoop.time < 0.1f)
        {
            soundLoop.Stop();
            stopSound = false;
        }

        if(lifting && !soundLoop.isPlaying)
        {
            soundLoop.Play();
        }
    }

    void SetOutline(GameObject target)//move particles to target
    {
        try
        {
            if (!target.Equals(outline.transform.parent.gameObject)) outline.Clear();
        }
        catch
        {

        }
        ParticleSystem.ShapeModule shape = outline.shape;

        //MeshFilter filter = target.GetComponent<MeshFilter>();

        //shape.mesh = filter.mesh;
        OutlineRadius rad = target.GetComponent<OutlineRadius>();

        if(rad != null)
        {
            shape.radius = rad.radius;
        }
        else
        {
            shape.radius = 1;
        }

        outline.transform.SetParent(target.transform);
        outline.transform.localPosition = Vector3.zero;
        outline.transform.localEulerAngles = Vector3.zero;
        outline.Play();
    }

    void Targeting()
    {
        //scan for targets
        if (!lifting && !grabbing)
        {
            liftTarget = null;
            grabTarget = null;

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
                Item item = canidate.collider.transform.gameObject.GetComponentInParent<Item>();
                int layer = canidate.collider.gameObject.layer;

                if (layer == LayerMask.NameToLayer(ignoreLayer) && item.isHeld)
                {
                    continue;
                }
                else
                {
                    theOne = canidate.collider.attachedRigidbody;

                    //prioritize items
                    if (item != null)
                    {
                        break;
                    }

                    if (theOne && theOne.mass > highestMass)
                    {
                        highestMass = theOne.mass;
                    }
                }
            }

            if (theOne != null)
            {
                SetOutline(theOne.gameObject);

                if (CompareTags(theOne.tag, grabTags))
                {
                    grabTarget = theOne;
                }
                else if (CompareTags(theOne.tag, liftTags) && !grabbing)
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
    }

    void Grab()
    {
        if (grabTarget && !IsHolding())//item
        {
            liftTarget = null;
            if (!grabbing)
            {
                OneshotManager.instance.PlaySound("psi_throw", transform.position);
            }

            lifting = true;
            grabbing = true;
            grabTarget.AddForce((hand.position - grabTarget.transform.position).normalized * grabForce, ForceMode.Acceleration);

            if (Vector3.Distance(hand.position, grabTarget.transform.position) < grabDistance)//is close enough to hand
            {
                //attatch to hand
                lifting = false;
                grabbing = false;
                handScript.AttachObject(grabTarget.gameObject, GrabTypes.Grip, attachmentFlags);
                stopSound = true;
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

    void LetGo()
    {
        lifting = false;
        joint.connectedBody = null;

        if (soundLoop.isPlaying)
        {
            stopSound = true;
            Debug.Log("stoping");
        }
    }

    void ResetOutline()//stop particles
    {
        outline.Stop();
        outline.Clear();
    }

    bool CompareTags(string tagIn, string[] tags)
    {
        foreach(string tag in tags)
        {
            if(tagIn.Equals(tag))
            {
                return true;
            }
        }
        return false;
    }

    bool IsHolding()
    {
        return hand.parent.GetComponentInChildren<Item>() != null;
    }
}