using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Teleport : MonoBehaviour
{
    public SteamVR_Input_Sources controller;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean snapTurnLeft;
    public SteamVR_Action_Boolean snapTurnRight;
    public Transform teleportHand;
    public TeleportArc arc;
    public LayerMask arcMask;
    public LayerMask sweepMask;
    public new CapsuleCollider collider;
    public Rigidbody body;

    private GameObject head;

    // Start is called before the first frame update
    void Start()
    {
        head = GlobalVars.Get("head");
        //arc.Hide();
        arc.traceLayerMask = arcMask;
    }
    RaycastHit hit = new RaycastHit();

    bool canTeleport = false;
    Vector3 dir, groundPos = new Vector3();
    //Vector3 a, b = new Vector3();

    void Update()
    {
        bool didHit = false;
        
        if (teleportAction.GetState(controller))
        {
            if (!arc.gameObject.activeSelf) arc.gameObject.SetActive(true);
            didHit = DrawArc(canTeleport);
            canTeleport = didHit ? CanTeleportTo(hit, body) : false;
            //arc.Show();
        }
        else if (teleportAction.GetStateUp(controller) && !snapTurnLeft.GetState(controller) && !snapTurnRight.GetState(controller))
        {
            //teleport here
            if (canTeleport)
            {
                Debug.Log("teleporting");

                //teleport with offset
                body.position += dir;
            }
            arc.gameObject.SetActive(false);
        }

        if(snapTurnLeft.GetState(controller) || snapTurnRight.GetState(controller))
        {
            arc.gameObject.SetActive(false);
        }
    }

    bool DrawArc(bool canTeleport)
    {
        arc.SetArcData(teleportHand.position, teleportHand.forward * 10, true, false);
        bool didHit = arc.DrawArc(out hit);
        //Debug.Log(hit.collider.gameObject);
        arc.SetColor(canTeleport ? Color.cyan : Color.red);
        return didHit;
    }

    //can we fit to the destination?
    bool CanTeleportTo(RaycastHit hitInfo, Rigidbody body)
    {
        if (!hitInfo.collider.gameObject.layer.Equals(LayerMask.NameToLayer("NavMesh"))) return false;

        groundPos = GetGroundPoint();
        dir = hitInfo.point - groundPos;

        RaycastHit cHit;
        if(Physics.SphereCast(head.transform.position, 0.125f, dir, out cHit, dir.magnitude, sweepMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(cHit.collider.gameObject);
            return false;
        }

        return true;
    }

    Vector3 GetGroundPoint()
    {
        RaycastHit groundHit = new RaycastHit();

        Physics.Raycast(head.transform.position, Vector3.down, out groundHit, 2f, arcMask);
        //Debug.Log(groundHit.collider);
        return groundHit.point;
    }
}
