using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class Robot : MonoBehaviour, IAttackAgent
{
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
