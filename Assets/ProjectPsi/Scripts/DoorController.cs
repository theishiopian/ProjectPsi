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

    private bool detected = false;
    private float t = 0;//0-1 closed-open
    private DoorState state = DoorState.CLOSED;

    // Update is called once per frame
    void Update()
    {
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
        detected = true;
    }

    public void OnExit(Collider other)
    {
        detected = false;
    }

    public void OnStay(Collider other)
    {
        detected = true;
    }

}
