using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum DoorState
{
    OPEN, CLOSED, LOCKED
}

public class DoorController : MonoBehaviour, ITriggerListener
{
    public Transform door;
    public Vector3 openPos;
    public AnimationCurve openCurve;
    public AnimationCurve closeCurve;

    private bool moving = false;
    private float t = 0;//0-1 closed-open
    private DoorState state = DoorState.CLOSED;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            switch (state)
            {
                case DoorState.CLOSED://open door
                    {
                        MoveDoorPos(t, openCurve);
                    }
                    break;
                case DoorState.OPEN://close door
                    {
                        MoveDoorPos(t, closeCurve);
                    }
                    break;
            }
            t += Time.deltaTime;
            if(t >=1)
            {
                moving = false;
                t = 0;
                ToggleState();
            }
        }

    }

    void MoveDoorPos(float t, AnimationCurve curve)
    {
        door.localPosition = Vector3.Lerp(Vector3.zero, openPos, curve.Evaluate(t));
    }

    void ToggleState()
    {
        //Debug.Log("toggling");
        if(state == DoorState.CLOSED)
        {
            state = DoorState.OPEN;
            return;
        }
        if (state == DoorState.OPEN)
        {
            state = DoorState.CLOSED;
            return;
        }
    }

    public void OnEnter(Collider other)
    {
        if(state != DoorState.LOCKED && !moving)
        {
            moving = true;
            //ToggleState();
        }
    }

    public void OnExit(Collider other)
    {
        if (state != DoorState.LOCKED && !moving)
        {
            moving = true;
            //ToggleState();
        }
    }

    public void OnStay(Collider other)
    {
        //throw new System.NotImplementedException();
    }

}
