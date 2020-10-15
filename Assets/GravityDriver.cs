using System.Collections;
using System.Collections.Generic;
using Unity.Labs.SuperScience;
using UnityEngine;
using Valve.VR;

public class GravityDriver : MonoBehaviour
{
    public Transform head;

    public Transform hand;

    public SteamVR_Input_Sources controller;
    public SteamVR_Action_Boolean trigger;

    public GameObject gravitySphere;

    public float speedFactor = 10f;

    private PhysicsTracker handTracker;

    // Start is called before the first frame update
    void Start()
    {
        handTracker = new PhysicsTracker();
    }

    private RaycastHit hit;

    private void FixedUpdate()
    {
        handTracker.Update(hand.position, hand.rotation, Time.fixedDeltaTime);
        if (trigger.GetStateDown(controller))
        {
            if (Physics.Raycast(head.position, head.forward, out hit, 10f))
            {
                gravitySphere.transform.position = hit.point;
                gravitySphere.SetActive(true);
            }
        }

        if (trigger.GetState(controller))
        {
            gravitySphere.transform.position += handTracker.Velocity * Time.fixedDeltaTime * speedFactor;
        }

        if (trigger.GetStateUp(controller))
        {
            gravitySphere.SetActive(false);
        }
    }
}
