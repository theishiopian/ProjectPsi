using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public TrailRenderer trail;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        trail.emitting = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("clear");
        trail.emitting = false;
        trail.Clear();
    }
}
