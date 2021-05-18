using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocket : MonoBehaviour, ITriggerListener
{
    public Valve.VR.InteractionSystem.Hand otherHand;

    private GameObject icon;

    private GameObject storedItem = null;

    private Transform parent;

    private void Start()
    {
        Pause.OnPause += OnPause;
        Menu.OnPlay += OnPause;
        Menu.OnResume += OnPause;
        icon = transform.Find("PocketIcon").gameObject;
        parent = transform.Find("PocketParent");
        icon.SetActive(false);
    }

    private void Update()
    {
        icon.SetActive(otherHand.currentAttachedObject != null);
    }

    public void OnEnter(Collider other)
    {
        icon.transform.localScale = Vector3.one * 1.3f * 0.1f;
    }

    public void OnExit(Collider other)
    {
        icon.transform.localScale = Vector3.one * 0.1f;
    }

    public void OnStay(Collider other)
    {
        if(storedItem == null)
        {
            storedItem = other.transform.gameObject;
            if (!storedItem.GetComponent<Item>().isHeld && !storedItem.GetComponent<Item>().isStored)
            {
                Debug.Log("storing");
                storedItem.transform.parent = parent;
                storedItem.transform.localPosition = Vector3.zero;
                storedItem.GetComponent<Rigidbody>().isKinematic = true;
            }
            else storedItem = null;
        }
        else if(storedItem != null && storedItem.GetComponent<Item>().isHeld)
        {
            Debug.Log("unstoring");
            storedItem.GetComponent<Rigidbody>().isKinematic = false;

            storedItem = null;
        }
    }

    void OnPause()
    {
        if(storedItem)
        {
            storedItem.transform.parent = null;
            storedItem.GetComponent<Rigidbody>().isKinematic = false;
            storedItem = null;
        }
        //the funny way to make items get dropped from hand
        if(otherHand.AttachedObjects.Count > 0)
        {
            otherHand.DetachObject(otherHand.AttachedObjects[0].attachedObject);
        }
    }
}
