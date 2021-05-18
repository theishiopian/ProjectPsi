using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Item : MonoBehaviour
{
    public bool debug = false;
    public bool isStored = false;
    public bool isHeld = false;

    private void Update()
    {
        if(debug)
        {
            Debug.Log("IsStored: " + isStored);
            Debug.Log("IsHeld: " + isHeld);
        }
    }

    private void OnTransformParentChanged()
    {
        if(transform.parent != null)
        {
            GameObject parent = transform.parent.gameObject;
            if (parent.GetComponent<Hand>())
            {
                isHeld = true;
                isStored = false;
            }
            else if (parent.transform.parent != null && parent.transform.parent.GetComponent<Pocket>())
            {
                isStored = true;
                isHeld = false;
            }
            else
            {
                isStored = false;
                isHeld = false;
            }
        }
        else
        {
            isStored = false;
            isHeld = false;
        }
    }
}
