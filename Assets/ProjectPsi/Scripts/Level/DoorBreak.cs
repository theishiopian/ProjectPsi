using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBreak : AbstractHealth
{
    public float armor = 30;

    ParticleSystem shatter;

    private void Start()
    {
        shatter = GlobalVars.Get("door_shatter").GetComponent<ParticleSystem>();
        onDeath = OnDeath;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > armor)
        {
            Damage(collision.impulse.magnitude * 0.01f);
            onDeath.Invoke();
        }
    }

    private void OnDeath()
    {

    }
}
