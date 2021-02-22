using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class PlayerHealth : AbstractHealth
{
    [Header("Tags (WARNING do not change at runtime)")]
    public const string stunTag = "stun";
    public const string killTag = "bullet";

    [Header("Player Health Settings")]
    public float regenRate = 10;//regen rate of health  per second
    public float gracePeriod = 0.5f;//during grace, health doesnt regen but you cant take damage either
    public float maxStunTime;
    
    private bool isAlive = true;//is the player alive?
    private float graceTimer = 0;//how much grace is left?

    public override void Damage(float amount)//deal damage, implemented from IDamageable
    {
        base.Damage(amount);        
        graceTimer = gracePeriod;
    }

    public void Stun()//stun halves movement speed and teleport range
    {
        //TODO
    }

    void Awake()
    {
        //TODO register with global vars for scientists
    }

    void Start()
    {
        Health = startingHealth;
    }

    void Update()
    {
        graceTimer = Mathf.Clamp(0, gracePeriod, graceTimer - Time.deltaTime);

        if(graceTimer <=0)
        {
            Health = Mathf.Clamp(0, startingHealth, Health + regenRate * Time.deltaTime);
        }
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
                        Damage(10);
                        break;
                    }
            }
        }

    }
}
