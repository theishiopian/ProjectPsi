using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuTrigger : MonoBehaviour
{
    [System.Serializable]
    public class MenuCallback : UnityEvent { }

    public MenuCallback OnDrawn;
    public MenuCallback OnErased;

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Marker"))
        {
            Debug.Log("marker detected");
            OnDrawn.Invoke();
        }
        else if((other.CompareTag("Eraser")))
        {
            Debug.Log("eraser detected");
            OnErased.Invoke();
        }
    }
}
