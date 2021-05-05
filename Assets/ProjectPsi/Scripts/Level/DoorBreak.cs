using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBreak : AbstractHealth
{
    public float armor = 30;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > armor)
        {
            Damage(collision.impulse.magnitude * 0.01f);
        }
    }
}
