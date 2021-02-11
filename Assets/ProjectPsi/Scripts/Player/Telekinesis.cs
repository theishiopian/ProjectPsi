using System.Collections.Generic;
using Unity.Labs.SuperScience;
using UnityEngine;
using Valve.VR;
using cakeslice;

public class Telekinesis : MonoBehaviour
{
    [Header("SteamVR")]
    public SteamVR_Input_Sources controller;
    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Action_Boolean adjustAction;

    [Header("Transforms")]
    public Transform head;
    public Transform body;
    public Transform hand;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}