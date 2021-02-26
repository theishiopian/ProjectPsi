using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : AbstractHealth
{
    public const string stunTag = "Stun";
    public const string killTag = "Kill";

    [Header("Player Health Settings")]
    public float regenRate = 10;//regen rate of health  per second
    public float gracePeriod = 0.5f;//during grace, health doesnt regen but you cant take damage either
    public bool overrideStartingHealth = false;
    public float overrideHealth = 15;
    public float maxStunTime;

    [Header("Post Processing Settings")]
    public bool doPostProcess = true;
    public PostProcessVolume volume;
    public AnimationCurve heartBeatCurve;
    public float beatDuration = 2;
    public float beatDelay = 1f;

    private ColorGrading colorEffects;
    private ChromaticAberration damageEffects;
    private Vignette vinEffects;//not spelling that every time. still

    public override void Damage(float amount)//deal damage, implemented from IDamageable
    {
        graceTimer = gracePeriod;
        
        base.Damage(amount);        
    }

    public void Stun()//stun halves movement speed and teleport range
    {
        //TODO
    }

    private bool GetEffects()
    {
        return
            volume.profile.TryGetSettings(out colorEffects)
            && volume.profile.TryGetSettings(out damageEffects)
            && volume.profile.TryGetSettings(out vinEffects);
    }

    void Start()
    {
        Health = startingHealth;

        if(!volume)
        {
            doPostProcess = false;
            Debug.LogWarning("No post-proccess volume detected, disabling post proccessing effects");
        }
        else if(!GetEffects())
        {
            doPostProcess = false;
            Debug.LogWarning("Attempt to get post process settings failed, disabling post proccessing effects");
        }

        if(overrideStartingHealth)
        {
            Health = overrideHealth;
        }
    }

    private float graceTimer = 0;//how much grace is left?
    private float heartBeatTimer = 0;

    private bool hasBeat = true;

    void Update()
    {
        graceTimer = Mathf.Clamp(graceTimer - Time.deltaTime, 0, gracePeriod);
        
        if (graceTimer <= 0)
        {
            Health = Mathf.Clamp(Health + regenRate * Time.deltaTime, 0, startingHealth);
        }

        if(doPostProcess)
        {
            damageEffects.intensity.value = Mathf.Clamp(damageEffects.intensity.value - Time.deltaTime * 2.5f, 0, 1);
            colorEffects.saturation.value = Health.Remap(0, startingHealth, -startingHealth + 25, 0);
            colorEffects.contrast.value = Health.Remap(0, startingHealth, startingHealth, 0);

            if (Health <= startingHealth / 5)
            {
                heartBeatTimer = Mathf.Clamp(heartBeatTimer - Time.deltaTime, 0, hasBeat ? beatDelay : beatDuration);

                //health/starting

                if (heartBeatTimer <= 0)
                {
                    hasBeat = !hasBeat;
                    heartBeatTimer = hasBeat ? beatDelay : beatDuration;
                }

                if (!hasBeat)
                {
                    vinEffects.intensity.value = heartBeatCurve.Evaluate(heartBeatTimer.Remap(0, beatDuration, 0, 1));
                }
            }
            else
            {
                vinEffects.intensity.value = 0;
            }
        }

        Debug.LogFormat("Value: {0}", Health);
        //Debug.LogFormat("Health: {0}, Grace: {1}", Health, graceTimer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(graceTimer <= 0)
        {
            switch (collision.collider.tag)
            {
                case stunTag:
                    {
                        //stun
                        break;
                    }
                case killTag:
                    {
                        Damage(collision.collider.GetComponent<ProjectileData>().damage);
                        if(doPostProcess)damageEffects.intensity.value = 1;
                        break;
                    }
            }
        }

    }
}
