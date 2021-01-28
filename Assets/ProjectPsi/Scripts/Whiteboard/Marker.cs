using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public TrailRenderer trail;
    public LayerMask mask;

    private void LateUpdate()
    {
        
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (Physics.OverlapSphere(transform.position, 0.03f, mask).Length > 0)
    //    {
    //        trail.emitting = true;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    trail.emitting = false;
    //    trail.Clear();
    //}
}
