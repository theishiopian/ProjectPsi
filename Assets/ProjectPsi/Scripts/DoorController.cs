using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum DoorState
{
    OPEN, CLOSED, LOCKED, OPENING, CLOSING
}

public class DoorController : MonoBehaviour, ITriggerListener
{
    public Transform door;
    public Vector3 openPos;
    public AnimationCurve openCurve;
    public AnimationCurve closeCurve;

    //private bool moving = false;
    private bool detected = false;
    private float t = 0;//0-1 closed-open
    private DoorState state = DoorState.CLOSED;

    // Update is called once per frame
    void Update()
    {
        //if(moving)
        //{
        //    switch (state)
        //    {
        //        case DoorState.CLOSING://open door
        //            {
        //                MoveDoorPos(t, closeCurve);
        //            }
        //            break;
        //        case DoorState.OPENING://close door
        //            {
        //                MoveDoorPos(t, openCurve);
        //            }
        //            break;
        //    }
        //    t += Time.deltaTime;
        //}

        //Debug.Log(t);
        //if (moving && t >= 1)
        //{
        //    moving = false;
        //    t = 0;

        //    switch (state)
        //    {
        //        case DoorState.OPENING: state = DoorState.OPEN; break;
        //        case DoorState.CLOSING: state = DoorState.CLOSED; break;
        //    }
        //}

        switch (state)
        {
            case DoorState.CLOSED:
                {
                    if(detected)
                    {
                        state = DoorState.OPENING;
                    }
                }
                break;
            case DoorState.OPENING:
                {
                    MoveDoorPos(openCurve);
                    if(t>=1)
                    {
                        state = DoorState.OPEN;
                        t = 0;
                    }
                }
                break;
            case DoorState.OPEN:
                {
                    if(!detected)
                    {
                        state = DoorState.CLOSING;
                    }
                }
                break;
            case DoorState.CLOSING:
                {
                    MoveDoorPos(closeCurve);
                    if (t >= 1)
                    {
                        state = DoorState.CLOSED;
                        t = 0;
                    }
                }
                break;
            case DoorState.LOCKED:
                {
                    //key check here
                }
                break;
        }
    }

    void MoveDoorPos(AnimationCurve curve)
    {
        door.localPosition = Vector3.Lerp(Vector3.zero, openPos, curve.Evaluate(t));
        t += Time.deltaTime;
    }

    public void OnEnter(Collider other)
    {
        //if(!moving)
        //{
        //    state = DoorState.OPENING;
        //    moving = true;
        //}
        detected = true;
    }

    public void OnExit(Collider other)
    {
        //if(!moving)
        //{
        //    state = DoorState.CLOSING;
        //    moving = true;
        //}
        detected = false;
    }

    public void OnStay(Collider other)
    {
        //throw new System.NotImplementedException();
        detected = true;
    }

}
