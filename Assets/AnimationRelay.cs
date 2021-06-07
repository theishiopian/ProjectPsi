using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ToTrigger : UnityEvent<string> { }

[System.Serializable]
public class AnimationEvent
{
    public string key;
    public ToTrigger onTrigger;
}

public class AnimationRelay : MonoBehaviour
{
    public List<AnimationEvent> events;

    public void Trigger(string arg0)
    {
        foreach (AnimationEvent item in events)
        {
            if(arg0.Equals(item.key))
            {
                item.onTrigger.Invoke(arg0);
                return;
            }
        }
    }
}
