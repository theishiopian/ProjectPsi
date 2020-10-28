﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TriggerHelper : MonoBehaviour
{
    public MonoBehaviour target;
    public new string tag;

    private void Start()
    {
        if (!(target is ITriggerListener))
        {
            throw new System.ArgumentException(target.GetType().ToString());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag)) (target as ITriggerListener).OnEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tag)) (target as ITriggerListener).OnExit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(tag)) (target as ITriggerListener).OnStay(other);
    }
}
