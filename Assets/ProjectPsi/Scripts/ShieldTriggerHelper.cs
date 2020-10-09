using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTriggerHelper : MonoBehaviour
{
    public Shield target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("projectile")) target.OnShieldEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("projectile")) target.OnShieldExit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("projectile")) target.OnShieldStay(other);
    }
}
