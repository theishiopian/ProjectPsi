﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBreak : AbstractHealth
{
    public float armor = 30;

    ParticleSystem shatter;
    Vector3 impulse;

    private void Start()
    {
        shatter = GlobalVars.Get("door_shatter").GetComponent<ParticleSystem>();
        onDeath = OnDeath;
    }

    private void OnCollisionEnter(Collision collision)
    {
        impulse = collision.impulse;
        if (collision.impulse.magnitude > armor)
        {
            Damage(collision.impulse.magnitude * 0.01f);
            OneshotManager.instance.PlaySound("door_shatter", transform.position,
                new SoundParams
                {
                    pitch = Random.Range(0.8f, 1.1f),
                    volume = 1
                }
                );
            onDeath.Invoke();
        }
    }

    private void OnDeath()
    {
        shatter.transform.position = transform.position;
        shatter.transform.rotation = Quaternion.LookRotation(impulse, transform.up);
        shatter.Play();
    }
}
