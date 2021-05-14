﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Objects")]
    public ParticleSystem bullets;
    public ParticleSystem muzzleflash;
    public ParticleSystem shells;
    public Transform shootPoint;
    public Transform bracket;
    public Transform gun;

    [Header("Tracking Settings")]
    public LayerMask losMask;

    private bool tracking = false;
    private bool firing = false;
    private Transform target;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tracking)
        {
            gun.rotation = Quaternion.Lerp(gun.rotation, Quaternion.LookRotation((target.position - transform.position).normalized, transform.up), Time.deltaTime * 3);
            bracket.rotation = Quaternion.Lerp(bracket.rotation, Quaternion.LookRotation((target.position - transform.position).normalized, transform.up), Time.deltaTime * 3);
            bracket.localEulerAngles = new Vector3(0, bracket.localEulerAngles.y, 0);
            gun.localEulerAngles = new Vector3(gun.localEulerAngles.x, -90, 0);

            RaycastHit hit;

            bool didHit = Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, losMask);

            if(didHit && hit.collider.CompareTag("Player"))
            {
                if(!firing)StartFiring();
            }
            else
            {
                if(firing)StopFiring();
            }
        }
        else
        { 
            if(firing)StopFiring();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            tracking = true;
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tracking = false;
            target = null;
        }
    }

    private void StartFiring()
    {
        muzzleflash.Play();
        shells.Play();
        bullets.Play();
        firing = true;
    }

    private void StopFiring()
    {
        muzzleflash.Stop();
        shells.Stop();
        bullets.Stop();
        firing = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Player"))Debug.Log("Hit");
    }
}