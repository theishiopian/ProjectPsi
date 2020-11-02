using System.Collections.Generic;
using Unity.Labs.SuperScience;
using UnityEngine;
using Valve.VR;

public class Telekinesis : MonoBehaviour
{
    public SteamVR_Input_Sources controller;//TODO vr input manager
    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Action_Boolean adjustAction;

    private Transform head;
    private bool moving = false;
    private PhysicsTracker tracker;
    private Psi psi;
    private Rigidbody cameraRigBody;
    private GameObject trackpoint;
    private readonly Vector3 trackpointOffset = new Vector3(0,0,3);

    private void Start()
    {
        psi = GlobalVars.playerPsi;
        head = GlobalVars.Get("head").transform;
        tracker = new PhysicsTracker();
        cameraRigBody = GlobalVars.Get("player_rig").GetComponent<Rigidbody>();

        trackpoint = new GameObject("trackPoint");

        trackpoint.transform.parent = head;
        trackpoint.transform.localPosition = trackpointOffset;
        trackpoint.transform.localRotation = Quaternion.identity;
        trackpoint.transform.localScale = Vector3.one;
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.parent = trackpoint.transform;
        cube.GetComponent<Collider>().enabled = false;
        cube.transform.localPosition = Vector3.zero;
        cube.transform.localRotation = Quaternion.identity;
    }

    private Vector3 additiveVelocity;
    private GameObject cube;//temp object for testing

    private void FixedUpdate()
    {
        tracker.Update(transform.localPosition, transform.localRotation, Time.fixedDeltaTime);
        
        if(triggerAction.GetStateDown(controller))
        {
            GetTargets();
        }

        if(triggerAction.GetState(controller))
        {
            cube.SetActive(true);
            if (adjustAction.GetState(controller))
            {
                additiveVelocity = tracker.Velocity * Time.fixedDeltaTime * 3;
                //additiveVelocity.y = 0;
                trackpoint.transform.position += additiveVelocity;
            }
            else
            {
                trackpoint.transform.localPosition = Vector3.Slerp(trackpoint.transform.localPosition, trackpointOffset, Time.fixedDeltaTime);
            }

            MoveTargets();
        }
        else
        {
            cube.SetActive(false);
            trackpoint.transform.localPosition = trackpointOffset;
            targets = new List<Rigidbody>();
        }
    }

    private RaycastHit hit;
    private List<Rigidbody> targets = new List<Rigidbody>();
    private Collider[] potentialTargets;

    Rigidbody potentialBody;

    void GetTargets()
    {
        if (Physics.Raycast(head.position, head.forward, out hit, 15))
        {
            potentialTargets = Physics.OverlapSphere(hit.point, 1f);

            foreach(Collider c in potentialTargets)
            {
                potentialBody = c.GetComponent<Rigidbody>();

                if(potentialBody != null && c.CompareTag("Grabbable"))
                {
                    targets.Add(potentialBody);
                }
            }
        }
        else targets = new List<Rigidbody>();
    }

    void MoveTargets()
    {
        if(targets != null)
        {
            foreach(Rigidbody body in targets)
            {
                body.AddForce((trackpoint.transform.position - body.transform.position).normalized * 100, ForceMode.Acceleration);
            }
        }
    }
}