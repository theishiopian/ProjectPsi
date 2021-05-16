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
        if(!other.transform.gameObject.GetComponent<Item>().isHeld && storedItem == null)
        {
            storedItem = other.transform.gameObject;
            storedItem.transform.parent = parent;
            storedItem.transform.localPosition = Vector3.zero;
            storedItem.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if(storedItem != null)
        {
            if (storedItem.GetComponent<Item>().isHeld)
            {
                storedItem.transform.parent = null;
                storedItem.GetComponent<Rigidbody>().isKinematic = false;
                storedItem = null;
            }
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
        //the funny
        if(otherHand.AttachedObjects.Count > 0)
        {
            otherHand.DetachObject(otherHand.AttachedObjects[0].attachedObject);
        }
    }
}
