using System.Collections.Generic;
using Unity.Labs.SuperScience;
using UnityEngine;
using Valve.VR;
using cakeslice;

public class Telekinesis : MonoBehaviour
{
    public SteamVR_Input_Sources controller;//TODO vr input manager
    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Action_Boolean adjustAction;
    public LayerMask layers;

    private Transform head;
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
        else GetTargets();

        if (triggerAction.GetStateUp(controller))
        {
            //cube.SetActive(false);
            trackpoint.transform.localPosition = trackpointOffset;
            //Debug.Log(targets.Count);

            ResetRigidbodies(targets);

            targets = new List<Rigidbody>();
        }
    }

    private RaycastHit hit;
    private List<Rigidbody> targets = new List<Rigidbody>();
    private Collider[] potentialTargets;

    Rigidbody potentialBody;

    void GetTargets()
    {
        if (Physics.SphereCast(head.position, 0.5f, head.forward, out hit, 50, layers.value))
        {
            potentialTargets = Physics.OverlapSphere(hit.point, 0.5f, layers.value, QueryTriggerInteraction.Ignore);
            ResetRigidbodies(targets);
            targets.Clear();
            foreach (Collider c in potentialTargets)
            {
                potentialBody = c.GetComponent<Rigidbody>();
                //Debug.Log(psi.GetPsi());

                if (potentialBody != null && potentialBody.mass/10 <= psi.GetPsi())
                {
                    targets.Add(potentialBody);
                    Outline o = potentialBody.gameObject.GetComponent<Outline>();
                    
                    if(o == null)
                    {
                        potentialBody.gameObject.AddComponent<Outline>();
                    }
                    else
                    {
                        o.enabled = true;
                    }
                }
            }
        }
        else
        {
            ResetRigidbodies(targets);
            targets = new List<Rigidbody>();
        }
    }

    void MoveTargets()
    {
        if(targets != null)
        {
            foreach(Rigidbody body in targets)
            {
                if(body.drag == 0) body.drag = 5;
                body.AddForce(((trackpoint.transform.position - body.transform.position).normalized * 200) + new Vector3(0, 9.8f, 0), ForceMode.Acceleration);
                //Debug.Log(body);
            }
        }
    }

    void ResetRigidbodies(List<Rigidbody> bodies)
    {
        foreach(Rigidbody body in bodies)
        {
            body.drag = 0;
            body.gameObject.GetComponent<Outline>().enabled = false;
        }
    }
}