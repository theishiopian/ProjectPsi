using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : MonoBehaviour, ITriggerListener
{
    public Light indicator;
    public bool locked = true;
    public string code;
    public float blinkTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float t = 0;

    // Update is called once per frame
    void Update()
    {
        if (locked)
        {
            t += Time.deltaTime;

            if(t >= blinkTime)
            {
                t = 0;
                indicator.enabled = indicator.enabled ? false : true;
            }
        }
        else
        {
            indicator.enabled = true;
            this.enabled = false;
        }
    }

    public void OnEnter(Collider other)
    {
        Key key = other.gameObject.GetComponent<Key>();
        if(key != null && key.code == code)
        {
            locked = false;
        }
    }

    public void OnExit(Collider other)
    {
        //throw new System.NotImplementedException();
    }

    public void OnStay(Collider other)
    {
        //throw new System.NotImplementedException();
    }
}
