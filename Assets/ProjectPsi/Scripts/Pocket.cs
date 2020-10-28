using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocket : MonoBehaviour, ITriggerListener
{
    private GameObject icon;

    private GameObject storedItem = null;

    private Transform parent;

    private void Start()
    {
        icon = transform.Find("PocketIcon").gameObject;
        Debug.Log(icon);
        parent = transform.Find("PocketParent");
    }

    public void OnEnter(Collider other)
    {
        Debug.Log("entered");
        icon.SetActive(true);
        icon.transform.localScale = Vector3.one * 1.3f * 0.1f;
    }

    public void OnExit(Collider other)
    {
        Debug.Log("exited");
        icon.SetActive(true);
        icon.transform.localScale = Vector3.one * 0.1f;
    }

    public void OnStay(Collider other)
    {
        if(!other.transform.parent.gameObject.GetComponent<Item>().isHeld && storedItem == null)
        {
            storedItem = other.transform.parent.gameObject;
            storedItem.transform.parent = parent;
            storedItem.transform.localPosition = Vector3.zero;
            storedItem.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if(storedItem != null)
        {
            if (storedItem.GetComponent<Item>().isHeld)
            {
                icon.SetActive(true);
                storedItem.transform.parent = null;
                storedItem.GetComponent<Rigidbody>().isKinematic = false;
                storedItem = null;
            }
            else
            {
                icon.SetActive(false);
            }
        }
    }
}
