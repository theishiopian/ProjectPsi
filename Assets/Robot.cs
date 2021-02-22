using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class Robot : AbstractHealth, IAttackAgent
{
    [Header("Objects")]
    public GameObject projectilePrefab;

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
