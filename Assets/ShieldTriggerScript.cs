using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTriggerScript : MonoBehaviour
{
    public Shield target;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("projectile"))
        {
            target.OnShieldTrigger(other);
        }
    }
}
