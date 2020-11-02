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
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.parent = trackpoint.transform;
        cube.GetComponent<Collider>().enabled = false;
        cube.transform.localPosition = Vector3.zero;
        cube.transform.localRotation = Quaternion.identity;
    }

    private RaycastHit hit;
    private Rigidbody[] targets;
    private Vector3 additiveVelocity;

    private void FixedUpdate()
    {
        tracker.Update(transform.localPosition, transform.localRotation, Time.fixedDeltaTime);
        
        if(adjustAction.GetState(controller))
        {
            additiveVelocity = tracker.Velocity * Time.fixedDeltaTime * 3;
            //additiveVelocity.y = 0;
            trackpoint.transform.position += additiveVelocity;
        }
        else
        {
            trackpoint.transform.localPosition = Vector3.Slerp(trackpoint.transform.localPosition, trackpointOffset, Time.fixedDeltaTime);
        }
    }
}