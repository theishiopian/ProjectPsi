using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Tags (WARNING do not change at runtime)")]
    public const string stunTag = "stun";
    public const string killTag = "bullet";

    [Header("Settings")]
    public float startingHealth = 100;
    public float gracePeriod;
    public float maxStunTime;
    
    public float Health { get; private set; }//health  value

    private bool isAlive = true;//is the player alive?

    public void Damage(float amount)//deal damage, implemented from IDamageable
    {
        Health -= amount;
    }

    public bool IsAlive()//get alive status, implemented from IDamageable
    {
        return isAlive;
    }

    public void Kill()//Instantly kill (environmental hazards, etc)
    {
        isAlive = false;
        Health = 0;
        gameObject.SetActive(false);
    }

    public void Stun()//stun halves movement speed and teleport range
    {
        //TODO
    }

    void Start()
    {
        Health = startingHealth;   
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
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
