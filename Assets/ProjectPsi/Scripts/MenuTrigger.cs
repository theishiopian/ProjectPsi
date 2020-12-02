using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuTrigger : MonoBehaviour
{
    [System.Serializable]
    public class MenuCallback : UnityEvent { }

    public MenuCallback callback;

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Marker"))
        {
            Debug.Log("detected");
            callback.Invoke();
        }
    }
}
