using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOutColl : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.gameObject);
    }
}
