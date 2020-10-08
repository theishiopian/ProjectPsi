using System.Collections;
using UnityEngine;
using Valve.VR;

public class Shield : MonoBehaviour
{
    public SteamVR_Input_Sources controller;
    public SteamVR_Action_Boolean triggerAction;//todo accessibility mode?

    private Collider shieldCollider;
    private ParticleSystem shieldParticles;
    private Transform head;
    private GameObject shield;
    private Psi psi;

    void Start()
    {
        psi = GlobalVars.playerPsi;
        head = GlobalVars.Get("head").transform;
        shield = GlobalVars.Get("shield");
        shieldCollider = shield.GetComponentInChildren<BoxCollider>();
        shieldParticles = shield.GetComponentInChildren<ParticleSystem>();
        Deactivate();
    }

    bool active = false;
    Vector3 heading, target, origin, calculatedPosition;

    void Update()
    {
        if (triggerAction.GetState(controller))
        {
            //activate
            if(!active)
            {
                Activate();
                active = true;
            }

            //move
            target = this.transform.position;
            origin = head.transform.position;

            target.y = 0;
            origin.y = 0;

            heading = target - origin;
            heading.Normalize();

            calculatedPosition = Vector3.zero;
            calculatedPosition = head.position + heading * (Vector3.Distance(target, origin) + 0.3f);
            calculatedPosition.y = transform.position.y;
            shield.transform.position = calculatedPosition;
            shield.transform.rotation = Quaternion.LookRotation(heading, Vector3.up);
        }
        else
        {
            //deactivate
            if (active)
            {
                Deactivate();
                active = false;
            }
        }
    }

    public void OnShieldTrigger(Collider collider)
    {
        Debug.Log("hit");
        psi.ModifyPsi(collider.GetComponent<Rigidbody>().velocity.magnitude);
        collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void Activate()
    {
        shieldCollider.enabled = true;
        shieldParticles.Play();
    }

    void Deactivate()
    {
        shieldCollider.enabled = false;
        shieldParticles.Stop();
        shieldParticles.Clear();
    }
}