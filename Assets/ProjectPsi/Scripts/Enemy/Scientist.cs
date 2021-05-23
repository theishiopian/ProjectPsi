using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class Scientist : AbstractHealth, IAttackAgent
{
    [Header("AI Settings")]
    public float attackAngle = 25;
    public float attackDistance = 3;

    [Header("Combat Settings")]
    public float armor = 25;
    public Vector3 centerOfMass;
    public float stunTime = 1;
    public float killDelay = 1;//delay in seconds before jab
    public LayerMask attackMask;

    [Header("State Indication")]
    public MeshRenderer indicator;
    public Material attackMat;
    public Material wanderMat;
    public Material chaseMat;
    public Material stunMat;

    [HideInInspector]
    public GameObject player;

    private Rigidbody body;//used for physical responses
    private BehaviorTree ai;
    private NavMeshAgent agent;

    private float stunTimer = 0;
    private bool aiEnabled = true;
    private float killTimer = 0;
    private float waitTimer = 0;
    private bool attacking = false;

    //start tranq windup
    public void Attack(Vector3 targetPosition)
    {
        if(waitTimer <= 0)
        {
            Debug.Log("Sleep Attack");
            attacking = true;
            killTimer = 1;
        }
    }

    //inject tranq
    public void KillPlayer()
    {
        Debug.Log("jab");
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, 1, attackMask))
        {
            Debug.Log("hit target");
            hit.collider.attachedRigidbody.gameObject.GetComponent<PlayerHealth>().Kill();
        }

        waitTimer = 2;
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
        return !attacking && waitTimer <= 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GlobalVars.Get("player_body");
        Health = startingHealth;
        body = GetComponent<Rigidbody>();

        body.centerOfMass = centerOfMass;

        ai = GetComponent<BehaviorTree>();
        ai.SetVariableValue("Player", player);
        ai.enabled = true;

        agent = GetComponent<NavMeshAgent>();
    }

    public enum SciState
    {
        WANDERING,
        CHASING,
        ATTACKING,
        STUNNED
    }

    public SciState state = SciState.WANDERING;
    bool hasTarget = false;
    // Update is called once per frame
    void Update()
    {
        hasTarget = (bool)ai.GetVariable("HasTarget").GetValue();

        if (stunTimer > 0)
        {
            state = SciState.STUNNED;
        }
        else
        {
            if(hasTarget)
            {
                GameObject player = (GameObject)ai.GetVariable("Player").GetValue();

                if(!player)
                {
                    Debug.LogWarning("Player is null");
                }
                else
                {
                    if (Vector3.Distance(transform.position, (player.transform.position)) < attackDistance)
                    {
                        state = SciState.ATTACKING;
                    }
                    else
                    {
                        state = SciState.CHASING;
                    }
                }

                
            }
            else
            {
                state = SciState.WANDERING;
            }
        }

        switch (state)
        {
            case SciState.ATTACKING: indicator.material = attackMat; break;
            case SciState.CHASING: indicator.material = chaseMat; break;
            case SciState.WANDERING: indicator.material = wanderMat; break;
            case SciState.STUNNED: indicator.material = stunMat; break;
        }

        //Debug.Log(state);

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
        else
        {
            waitTimer = Mathf.Clamp(waitTimer - Time.deltaTime, 0, 1);
        }

        stunTimer = Mathf.Clamp(stunTimer- Time.deltaTime, 0, stunTime);
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
