using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class Robot : AbstractHealth, IAttackAgent
{
    [Header("Objects")]
    public GameObject projectilePrefab;//make pool

    [Header("AI Settings")]
    public float attackAngle = 45;
    public float attackDistance = 10;

    private List<GameObject> bulletPool;//store pool

    public void Attack(Vector3 targetPosition)
    {
        Debug.Log("Attack");
    }

    public float AttackAngle()
    {
        return attackAngle;
    }

    public float AttackDistance()
    {
        return attackDistance;
    }

    public bool CanAttack()
    {
        return true;
    }

    void Awake()
    {
        //do pool stuff here
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
