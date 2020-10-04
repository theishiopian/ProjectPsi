using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Psi : MonoBehaviour
{
    public SteamVR_Action_Boolean psiButton;

    public SteamVR_Input_Sources controllers;

    private bool psiActive = false;
    private float psiLevel = 10;

    private void Awake()
    {
        GlobalVars.playerPsi = this;
    }

    void Update()
    {
        psiActive = psiButton.GetState(controllers);
    }

    public bool GetPsiActive()
    {
        return psiActive;
    }

    public float GetPsi()
    {
        return psiLevel;
    }

    public void ModifyPsi(float amount)
    {
        psiLevel = Mathf.Clamp(psiLevel + amount, 10, 100);
    }
}
