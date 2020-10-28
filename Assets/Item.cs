using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isHeld
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        isHeld = false;
    }

    private void Update()
    {
        //Debug.Log(isHeld);
    }

    public void OnGrab()
    {
        isHeld = true;
    }

    public void OnRelease()
    {
        isHeld = false;
    }
}
