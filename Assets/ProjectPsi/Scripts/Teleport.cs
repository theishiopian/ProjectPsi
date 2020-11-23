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
    Vector3 dir, groundPos, a, b = new Vector3();
    // Update is called once per frame
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
        else if (teleportAction.GetStateUp(controller))
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
    }

    bool DrawArc(bool canTeleport)
    {
        arc.SetArcData(teleportHand.position, teleportHand.forward * 10, true, false);
        bool didHit = arc.DrawArc(out hit);

        arc.SetColor(canTeleport ? Color.cyan : Color.red);
        return didHit;
    }

    //can we fit to the destination?
    bool CanTeleportTo(RaycastHit hitInfo, Rigidbody body)
    {
        if (!hitInfo.collider.gameObject.layer.Equals(LayerMask.NameToLayer("NavMesh"))) return false;

        groundPos = GetGroundPoint();
        dir = hitInfo.point - groundPos;

        Debug.DrawRay(groundPos, Vector3.up, Color.magenta);

        Debug.DrawRay(groundPos, dir, Color.green);

        float pointDist = collider.height / 2 - collider.radius;
        a = new Vector3(0, collider.center.y, 0) + Vector3.up * pointDist;
        b = new Vector3(0, collider.center.y, 0) + Vector3.down * pointDist;
        RaycastHit cHit;
        if (Physics.CapsuleCast(groundPos + a, groundPos + b, collider.radius, dir, out cHit, dir.magnitude, sweepMask.value))
        {
            //Debug.Log("hit: " + cHit.collider);
            Debug.DrawRay(cHit.point, Vector3.up * 100);
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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(a + groundPos, 0.1f);
    //    Gizmos.DrawWireSphere(b + groundPos, 0.1f);
    //}
}
