using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class Robot : MonoBehaviour, IAttackAgent, IDamageable
{
    public float startingHealth = 50;
    public GameObject projectilePrefab;

    public float Health { get; private set;}

    private bool isAlive = true;

    public void Attack(Vector3 targetPosition)
    {
        Debug.Log("Attack");
    }

    public float AttackAngle()
    {
        return 45;
    }

    public float AttackDistance()
    {
        return 10;
    }

    public bool CanAttack()
    {
        return true;
    }

    public void Damage(float amount)
    {
        //throw new System.NotImplementedException();
        Health -= amount;

        if(Health <= 0)
        {
            isAlive = false;
            gameObject.SetActive(false);
        }
    }

    public void Kill()//Instantly kill (environmental hazards, etc)
    {
        isAlive = false;
        Health = 0;
        gameObject.SetActive(false);
    }

    public bool IsAlive()
    {
        //throw new System.NotImplementedException();
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
}
