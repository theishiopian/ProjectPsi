using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuTrigger : MonoBehaviour
{
    [System.Serializable]
    public class MenuCallback : UnityEvent<string> { }

    public MenuCallback OnDrawn;
    public MenuCallback OnErased;

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Marker"))
        {
            OnDrawn.Invoke("debug");
        }
        else if((other.CompareTag("Eraser")))
        {
            Debug.Log("eraser detected");
            OnErased.Invoke("debug");
        }
    }
}
