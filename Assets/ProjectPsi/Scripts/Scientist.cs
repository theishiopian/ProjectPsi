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
    public float attackAngle = 45;
    public float attackDistance = 10;
    public float reloadTime = 1;
    public float launchForce = 25;

    [Header("Object Pool Settings")]
    public GameObject projectilePrefab;//make pool
    public float cleanupTime = 20;
    public int poolSize = 5;

    [Header("Combat Settings")]
    public float armor = 25;
    public Vector3 centerOfMass;
    public float stunTime = 1;

    private Queue<GameObject> idlePool;
    private Queue<GameObject> activePool;

    private Rigidbody body;//used for physical responses
    private BehaviorTree ai;
    private NavMeshAgent agent;

    private float reloadTimer = 0;
    private float cleanupTimer = 0;
    private float stunTimer = 0;
    private bool aiEnabled = true;

    public void Attack(Vector3 targetPosition)
    {
        Debug.Log("Attack");
        reloadTimer = reloadTime;

        if(idlePool.Count == 0)
        {
            Cleanup();
        }

        GameObject toShoot = idlePool.Dequeue();
        toShoot.SetActive(true);
        toShoot.transform.parent = null;//todo master parent for robot?
        activePool.Enqueue(toShoot);

        toShoot.transform.rotation = gunPoint.rotation;
        toShoot.GetComponent<Rigidbody>().AddForce(toShoot.transform.forward * launchForce, ForceMode.Impulse);
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
        return reloadTimer <= 0 && stunTimer <=0;
    }

    void Awake()
    {
        //do pool stuff here
        idlePool = new Queue<GameObject>();
        activePool = new Queue<GameObject>();
        GameObject p;
        for (int i = 0; i < poolSize; i++)
        {
            p = Instantiate(projectilePrefab, gunPoint.position, gunPoint.rotation, gunPoint);
            idlePool.Enqueue(p);
            p.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = startingHealth;
        body = GetComponent<Rigidbody>();

        body.centerOfMass = centerOfMass;

        ai = GetComponent<BehaviorTree>();

        agent = GetComponent<NavMeshAgent>();
    }

    void Cleanup()
    {
        GameObject toCleanup = activePool.Dequeue();

        toCleanup.transform.parent = gunPoint;
        toCleanup.transform.localPosition = Vector3.zero;
        toCleanup.SetActive(false);

        idlePool.Enqueue(toCleanup);
    }

    // Update is called once per frame
    void Update()
    {
        reloadTimer = Mathf.Clamp(reloadTimer - Time.deltaTime, 0, reloadTime);
        cleanupTimer = Mathf.Clamp(cleanupTimer - Time.deltaTime, 0, cleanupTime);
        stunTimer = Mathf.Clamp(stunTimer - Time.deltaTime, 0, stunTime);

        if (cleanupTimer <=0)
        {
            cleanupTimer = cleanupTime;

            if(activePool.Count > 0)
            {
                Cleanup();
            }
        }

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
