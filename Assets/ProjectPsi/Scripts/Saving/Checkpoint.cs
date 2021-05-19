using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector3 pos;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            pos = other.transform.root.position;
            enabled = false;
        }
    }
}
