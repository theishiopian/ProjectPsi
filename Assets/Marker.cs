using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public float dist = 0.2f;

    private TrailRenderer trail;

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.up, dist, LayerMask.NameToLayer("WBCollision")))
        {
            trail.emitting = true;
            //Debug.Log("hit");
        }
        else
        {
            trail.emitting = false;
            trail.Clear();
        }
    }
}
