﻿using Unity.Labs.SuperScience;
using UnityEngine;
using Valve.VR;

public class Telekinesis : MonoBehaviour
{
    public SteamVR_Input_Sources controller;//TODO vr input manager
    public SteamVR_Action_Boolean triggerAction;//todo accessibility options?

    private Transform head;
    private RaycastHit hit;
    private bool moving = false;
    private Rigidbody target;
    private PhysicsTracker tracker;
    private Quaternion heading;

    private void Start()
    {
        head = GlobalVars.Get("head").transform;
        tracker = new PhysicsTracker();
    }

    private void FixedUpdate()
    {
        tracker.Update(transform.position, transform.rotation, Time.fixedDeltaTime);

        if(!moving)
        {
            //aquire target
            if (Physics.SphereCast(head.position, 0.3f, head.forward, out hit, 25f))
            {
                if (hit.collider.gameObject.CompareTag("Grabbable"))
                {
                    if (target != null) target.GetComponent<Outline>().enabled = false;
                    target = hit.collider.gameObject.GetComponent<Rigidbody>();
                    target.GetComponent<Outline>().enabled = true;
                }
            }

            if (triggerAction.GetState(controller))
            {
                moving = true;
            }
            
        }
        else if(target != null)
        {
            //actual velocity adding
            heading = Quaternion.Euler(0, head.eulerAngles.y, 0);//todo add secondary movement again?

            target.AddForce(Vector3.up * 9.8f, ForceMode.Acceleration);
            target.velocity = (tracker.Velocity/*add velocity here, multiply by heading*/) * Time.fixedDeltaTime * 500;

            if (!triggerAction.GetState(controller))
            {
                if (target != null)
                {
                    moving = false;
                    target.GetComponent<Outline>().enabled = false;
                    target = null;
                }
            }
        }

        if (target == null) moving = false;//this is a bandaid. a permenant fix is preferable but may never be found
    }
}