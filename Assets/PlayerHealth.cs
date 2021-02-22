using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Tags (WARNING do not change at runtime)")]
    public const string stunTag = "stun";
    public const string killTag = "bullet";
    public float startingHealth = 100;

    public float Health { get; private set; }

    public void Damage(float amount)
    {
        Health -= amount;
    }

    public bool IsAlive()
    {
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = startingHealth;   
    }

    // Update is called once per frame
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
