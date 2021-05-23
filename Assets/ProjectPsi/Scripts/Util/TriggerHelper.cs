using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TriggerHelper : MonoBehaviour
{
    public MonoBehaviour target;
    public string[] tags;
    public bool useTag = false;

    private void Start()
    {
        if (!(target is ITriggerListener))
        {
            throw new System.ArgumentException(target.GetType().ToString());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!useTag || CompareTags(other.tag, tags)) (target as ITriggerListener).OnEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!useTag || CompareTags(other.tag, tags)) (target as ITriggerListener).OnExit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!useTag || CompareTags(other.tag, tags)) (target as ITriggerListener).OnStay(other);
    }

    private bool CompareTags(string toCompare, string[] tags)
    {
        bool e = false;
        foreach (string tag in tags)
        {
            if (tag.Equals(toCompare))
            {
                e = true;
            }
        }

        return e;
    }
}
