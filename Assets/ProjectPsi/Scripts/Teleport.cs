using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Teleport : MonoBehaviour
{
    public SteamVR_Input_Sources controller;
    public SteamVR_Action_Boolean teleportAction;
    public Transform teleportHand;
    public TeleportArc arc;
    public LayerMask arcMask;
    public new CapsuleCollider collider;
    public Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        arc.Show();
        arc.traceLayerMask = arcMask;
    }
    RaycastHit hit;

    bool shouldTeleport = false;

    // Update is called once per frame
    void Update()
    {
        arc.SetArcData(teleportHand.position, teleportHand.forward * 10, true, false);
        bool didHit = arc.DrawArc(out hit);

        if(didHit && !hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("NavMesh")))
        {
            arc.SetColor(Color.red);
        }
        else
        {
            arc.SetColor(Color.blue);
        }
    }

    //can we fit to the destination?
    bool CanTeleportTo(Vector3 destination)
    {
        return true;
    }


}
