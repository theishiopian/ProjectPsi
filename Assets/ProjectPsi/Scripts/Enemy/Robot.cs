using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class Robot : AbstractHealth, IAttackAgent, IGun
{
    [Header("AI Settings")]
    public float attackAngle = 45;
    public float attackDistance = 10;
    public float launchForce = 25;

    [Header("Combat Settings")]
    public ParticleSystem gun;
    public float damage = 5;
    public float armor = 10;
    public Vector3 centerOfMass;
    public float stunTime = 2;

    //[Header("State Indication")]
    //public MeshRenderer indicator;
    //public Material attackMat;
    //public Material patrolMat;
    //public Material chaseMat;
    //public Material stunMat;

    [Header("Animation Setting")]
    public Animator animator;

    private Rigidbody body;//used for physical responses
    private BehaviorTree ai;
    private NavMeshAgent agent;

    private float stunTimer = 0;
    private bool aiEnabled = true;

    public void Attack(Vector3 targPosition)
    {
        //OneshotManager.instance.PlaySound("shotgun_fire", gun.transform.position);
        //gun.Play();
    }

    public void AttackNoise()
    {
        OneshotManager.instance.PlaySound("shotgun_fire", gun.transform.position);
    }

    //called by the particle system bridge
    public void Fire(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = GlobalVars.Get("player_rig").GetComponent<PlayerHealth>();

            health.Damage(damage);
        }
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
        Health = startingHealth;
        body = GetComponent<Rigidbody>();

        body.centerOfMass = centerOfMass;

        ai = GetComponent<BehaviorTree>();

        agent = GetComponent<NavMeshAgent>();

        ai.SetVariableValue("State", 0);
    }

    public enum RobotState
    {
        PATROLING,
        CHASING,
        ATTACKING,
        STUNNED
    }

    public RobotState state = RobotState.PATROLING;

    // Update is called once per frame
    void Update()
    {
        GameObject currentTarget = (GameObject)ai.GetVariable("CurrentTarget").GetValue();

        if(stunTimer > 0)
        {
            state = RobotState.STUNNED;
            animator.SetTrigger("AimDownTrigger");
        }
        else
        {
            if (currentTarget != null)
            {
                if (Vector3.Distance(currentTarget.transform.position, transform.position) < attackDistance)
                {
                    state = RobotState.ATTACKING;
                    animator.SetTrigger("AimUpTrigger");
                }
                else
                {
                    state = RobotState.CHASING;
                    animator.SetTrigger("WalkTrigger");
                }
            }
            else
            {
                state = RobotState.PATROLING;
                animator.SetTrigger("WalkTrigger");
            }
        }

        stunTimer = Mathf.Clamp(stunTimer - Time.deltaTime, 0, stunTime);

        if(stunTimer <= 0 && !aiEnabled)
        {
            EnableAI();
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
