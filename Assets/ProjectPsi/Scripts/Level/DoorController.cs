using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum DoorState
{
    OPEN, CLOSED, LOCKED, OPENING, CLOSING
}

public class DoorController : MonoBehaviour, ITriggerListener
{
    public bool startLocked;
    public Transform door;
    public Vector3 openPos;
    public AnimationCurve openCurve;
    public AnimationCurve closeCurve;
    public DoorPanel[] panels;

    private bool detected = false;
    private bool canOpen = true;
    private float t = 0;//0-1 closed-open
    DoorState state = DoorState.CLOSED;

    private void Start()
    {
        if (startLocked)
            state = DoorState.LOCKED;
    }

    public void Lock()
    {
        canOpen = false;
        state = DoorState.CLOSING;
        OneshotManager.instance.PlaySound("door_open", transform.position + openPos / 2);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case DoorState.CLOSED:
                {
                    if(detected && canOpen)
                    {
                        state = DoorState.OPENING;
                        OneshotManager.instance.PlaySound("door_open", transform.position + openPos/2);
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
                    if(!detected && canOpen)
                    {
                        state = DoorState.CLOSING;
                        OneshotManager.instance.PlaySound("door_open", transform.position + openPos / 2);
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
                    ////key check here
                    //if (!panel.locked)
                    //{
                    //    state = DoorState.CLOSED;
                    //}
                    int unlocked = 0;
                    for(int i = 0; i < panels.Length; i++)
                    {
                        if(!panels[i].locked)
                        {
                            unlocked++;
                        }
                    }

                    if (unlocked == panels.Length) state = DoorState.CLOSED;
                }
                break;
        }
    }

    void MoveDoorPos(AnimationCurve curve)
    {
        door.localPosition = Vector3.Lerp(Vector3.zero, openPos, curve.Evaluate(t));
        t += Time.deltaTime / 2;
    }

    public void OnEnter(Collider other)
    {
        if(canOpen)detected = true;
    }

    public void OnExit(Collider other)
    {
        if (canOpen) detected = false;
    }

    public void OnStay(Collider other)
    {
        if (canOpen) detected = true;
    }
}
