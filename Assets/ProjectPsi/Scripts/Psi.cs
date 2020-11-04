using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Psi : MonoBehaviour
{
    private float psiLevel = 10;

    private GameObject head;

    private void Awake()
    {
        GlobalVars.playerPsi = this;
    }

    float timer = 0;
    void Update()
    {
        if (timer <= 0) ModifyPsi(-5 * Time.deltaTime, false);
        else timer -= Time.deltaTime;
    }

    public float GetPsi()
    {
        return psiLevel;
    }

    public void ModifyPsi(float amount, bool delay)
    {
        if(delay)
        {
            timer = 2;
        }
        psiLevel = Mathf.Clamp(psiLevel + amount, 10, 100);//minimum amount 10
        Debug.Log("psi is now at: " + psiLevel);
    }
}
