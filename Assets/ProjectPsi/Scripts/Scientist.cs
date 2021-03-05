using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class Scientist : AbstractHealth, IAttackAgent
{
    [Header("Transforms")]
    public Transform gunPoint;

    [Header("AI Settings")]
    public float attackAngle = 25;
    public float attackDistance = 2;

    [Header("Combat Settings")]
    public float armor = 25;
    public Vector3 centerOfMass;
    public float stunTime = 1;
    public float killDelay = 1;//delay in seconds before jab

    [HideInInspector]
    public GameObject player;

    private Rigidbody body;//used for physical responses
    private BehaviorTree ai;
    private NavMeshAgent agent;

    private float stunTimer = 0;
    private bool aiEnabled = true;
    private float killTimer = 0;
    private bool attacking = false;

    //start tranq windup
    public void Attack(Vector3 targetPosition)
    {
        Debug.Log("Sleep Attack");
        attacking = true;
        killTimer = 1;
    }

    //inject tranq
    public void KillPlayer()
    {
        Debug.Log("jab");
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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("scientist start");
        player = GlobalVars.Get("player_body");
        Health = startingHealth;
        body = GetComponent<Rigidbody>();

        body.centerOfMass = centerOfMass;

        ai = GetComponent<BehaviorTree>();
        ai.SetVariableValue("Player", player);
        ai.enabled = true;

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stunTimer <= 0 && !aiEnabled)
        {
            EnableAI();
        }

        if(attacking)
        {
            if (killTimer <= 0)
            {
                KillPlayer();
                attacking = false;
            }
            else
            {
                killTimer -= Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.impulse.magnitude > armor)
        {
            if(aiEnabled)DisableAI();
            stunTimer = stunTime;

            Damage(collision.impulse.magnitude * 0.01f);
        }
    }

    private void EnableAI()
    {
        ai.enabled = true;
        body.isKinematic = true;
        agent.enabled = true;
        aiEnabled = true;
    }

    private void DisableAI()
    {
        ai.enabled = false;
        body.isKinematic = false;
        agent.enabled = false;
        aiEnabled = false;

    }
}
