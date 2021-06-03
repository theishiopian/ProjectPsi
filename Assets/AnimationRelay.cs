using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationRelay : MonoBehaviour
{
    [System.Serializable]
    public class ToTrigger : UnityEvent<string> { }

    public ToTrigger onTrigger;

    public void Trigger(string arg0)
    {
        onTrigger.Invoke(arg0);
    }
}
