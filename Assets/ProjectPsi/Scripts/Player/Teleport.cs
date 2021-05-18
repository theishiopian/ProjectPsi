using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Teleport : MonoBehaviour
{
    [Header("SteamVR")]
    public SteamVR_Input_Sources controller;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean snapTurnLeft;
    public SteamVR_Action_Boolean snapTurnRight;

    [Header("Objects")]
    public Transform teleportHand;
    public TeleportArc arc;
    public LineRenderer downLine;
    public new CapsuleCollider collider;
    public Rigidbody body;

    [Header("Settings")]
    public LayerMask arcMask;
    public LayerMask sweepMask;
    public float downLineOffset = 0.3f;

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
    Vector3 teleportVector, groundPos, hitPoint = new Vector3();
    //Vector3 a, b = new Vector3();

    void Update()
    {
        if (teleportAction.GetState(controller))
        {
            if (!arc.gameObject.activeSelf) arc.gameObject.SetActive(true);
            canTeleport = DoTeleportCheck();
            Color c = canTeleport? Color.cyan: Color.red;
            arc.SetColor(c);
            downLine.startColor = c;
            downLine.endColor = c;
        }
        else if (teleportAction.GetStateUp(controller) && !snapTurnLeft.GetState(controller) && !snapTurnRight.GetState(controller))
        {
            //teleport here
            if (canTeleport)
            {
                Debug.Log("teleporting");

                //teleport with offset
                body.position += teleportVector;
            }
            arc.gameObject.SetActive(false);
        }

        if(snapTurnLeft.GetState(controller) || snapTurnRight.GetState(controller))
        {
            arc.gameObject.SetActive(false);
        }
    }

    bool DoTeleportCheck()
    {
        arc.SetArcData(teleportHand.position, teleportHand.forward * 10, true, false);//TODO vary multiplier with angle
        bool didHit = arc.DrawArc(out hit);

        if(didHit)
        {
            if (!hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("NavMesh")))
            {
                groundPos = GetGroundPoint();
                hitPoint = hit.point;

                groundPos.y = 0;
                hitPoint.y = 0;

                Vector3 dir = (groundPos - hitPoint).normalized;
                Vector3 oldPoint = hit.point;
                Physics.Raycast(hit.point + dir * downLineOffset, Vector3.down, out hit, arcMask);

                if (!hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("NavMesh")))
                {
                    downLine.SetPosition(0, Vector3.zero);
                    downLine.SetPosition(1, Vector3.zero);
                    return false;
                }
                else
                {
                    downLine.SetPosition(0, oldPoint + dir * downLineOffset);
                    downLine.SetPosition(1, hit.point);
                }
            }

            groundPos = GetGroundPoint();
            teleportVector = hit.point - groundPos;

            RaycastHit cHit;
            if (Physics.SphereCast(head.transform.position, 0.125f, teleportVector, out cHit, teleportVector.magnitude, sweepMask, QueryTriggerInteraction.Ignore))
            {
                return false;
            }

            return true;
        }
        return false;
    }

    Vector3 GetGroundPoint()
    {
        RaycastHit groundHit = new RaycastHit();

        Physics.Raycast(head.transform.position, Vector3.down, out groundHit, 2f, arcMask);
        //Debug.Log(groundHit.collider);
        return groundHit.point;
    }
}
