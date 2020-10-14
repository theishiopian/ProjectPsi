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
    private ParticleSystem particles;
    private GameObject head;

    private void Awake()
    {
        GlobalVars.playerPsi = this;
        
    }

    private void Start()
    {
        head = GlobalVars.Get("head");
        particles = GetComponentInChildren<ParticleSystem>();
        Debug.Log(particles);
    }

    void Update()
    {
        psiActive = psiButton.GetState(controllers);
        Vector3 pos = head.transform.position;
        pos.y = this.transform.position.y;

        transform.position = pos;

        particles.emissionRate = psiLevel - 10;
        //Debug.Log(psiLevel);
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
        psiLevel = Mathf.Clamp(psiLevel + amount, 10, 100);//minimum amount 10
        Debug.Log("psi is now at: " + psiLevel);
    }
}
