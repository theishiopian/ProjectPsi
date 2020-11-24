using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MenuCallback();

public class MenuTrigger : MonoBehaviour
{
    public MenuCallback menuCallback;

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Marker"))
        {
            Debug.Log("detected");
            if(menuCallback != null)menuCallback();
        }
    }
}
