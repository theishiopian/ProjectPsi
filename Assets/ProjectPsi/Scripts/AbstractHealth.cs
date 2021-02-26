using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public abstract class AbstractHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float startingHealth = 100;//how much health does the player get on start?
    public bool isImmortal = false;

    public float Health { get; internal set; }//health  value

    private bool isAlive = true;//is the player alive?

    public virtual void Damage(float amount)//deal damage, implemented from IDamageable
    {
        Health -= amount;
        //Debug.Log("health at: " + Health);
        if (Health <= 0 && !isImmortal)
        {
            isAlive = false;
            gameObject.SetActive(false);
        }
    }

    public virtual bool IsAlive()//get alive status, implemented from IDamageable
    {
        return isAlive;
    }

    public virtual void Kill()//Instantly kill (environmental hazards, etc)
    {
        isAlive = false;
        Health = 0;
        gameObject.SetActive(false);
    }
}
