using System.Collections.Generic;
using Unity.Labs.SuperScience;
using UnityEngine;
using Valve.VR;

public class Telekinesis : MonoBehaviour
{
    public SteamVR_Input_Sources controller;//TODO vr input manager
    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Action_Boolean adjustAction;
    public LayerMask layers;

    private Transform head;
    private bool moving = false;
    private PhysicsTracker tracker;
    private Psi psi;
    private Rigidbody cameraRigBody;
    private GameObject trackpoint;
    private readonly Vector3 trackpointOffset = new Vector3(0,0,3);
    private Transform handAnchor;

    private void Start()
    {
        handAnchor = GlobalVars.Get("hand_anchor").transform;
        psi = GlobalVars.playerPsi;
        head = GlobalVars.Get("head").transform;
        tracker = new PhysicsTracker();
        cameraRigBody = GlobalVars.Get("player_rig").GetComponent<Rigidbody>();

        trackpoint = new GameObject("trackPoint");

        trackpoint.transform.parent = head;
        trackpoint.transform.localPosition = trackpointOffset;
        trackpoint.transform.localRotation = Quaternion.identity;
        trackpoint.transform.localScale = Vector3.one;
    }

    private Vector3 additiveVelocity;
    //private GameObject cube;//temp object for testing

    private void FixedUpdate()
    {
        tracker.Update(handAnchor.localPosition, handAnchor.localRotation, Time.fixedDeltaTime);
        
        if(triggerAction.GetStateDown(controller))
        {
            GetTargets();
        }

        if(triggerAction.GetState(controller))
        {
            //cube.SetActive(true);
            if (tracker.Speed > 0.2f)
            {
                additiveVelocity = tracker.Direction * Time.fixedDeltaTime * tracker.Speed * tracker.Speed * tracker.Speed;//cubed speed
                trackpoint.transform.localPosition += additiveVelocity;
            }
            else
            {
                trackpoint.transform.localPosition = Vector3.Slerp(trackpoint.transform.localPosition, trackpointOffset, Time.fixedDeltaTime * 5);
            }

            MoveTargets();
        }

        if(triggerAction.GetStateUp(controller))
        {
            //cube.SetActive(false);
            trackpoint.transform.localPosition = trackpointOffset;
            //Debug.Log(targets.Count);
            foreach (Rigidbody body in targets)
            {
                body.drag = 0;//todo save old drag value. struct?
                body.useGravity = true;
            }
            targets = new List<Rigidbody>();
        }
    }

    private RaycastHit hit;
    private List<Rigidbody> targets = new List<Rigidbody>();
    private Collider[] potentialTargets;

    Rigidbody potentialBody;

    void GetTargets()
    {
        if (Physics.SphereCast(head.position, 0.5f, head.forward, out hit, 50))
        {
            potentialTargets = Physics.OverlapSphere(hit.point, 0.5f, layers.value, QueryTriggerInteraction.Ignore);

            foreach (Collider c in potentialTargets)
            {
                potentialBody = c.GetComponent<Rigidbody>();

                if (potentialBody != null && c.CompareTag("Grabbable"))
                {
                    potentialBody.drag = 5;//todo save old drag value. struct?
                    potentialBody.useGravity = false;
                    targets.Add(potentialBody);
                    //Debug.Log(potentialBody);
                }
            }
        }
        else
        {
            targets = new List<Rigidbody>();
        }
    }

    void MoveTargets()
    {
        if(targets != null)
        {
            foreach(Rigidbody body in targets)
            {
                body.AddForce((trackpoint.transform.position - body.transform.position).normalized * 200, ForceMode.Acceleration);
                //Debug.Log(body);
            }
        }
    }
}