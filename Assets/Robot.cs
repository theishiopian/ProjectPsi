using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class Robot : AbstractHealth, IAttackAgent
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

    private Queue<GameObject> idlePool;
    private Queue<GameObject> activePool;

    private float reloadTimer = 0;
    private float cleanupTimer = 0;

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
        return reloadTimer <= 0;
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

        if(cleanupTimer <=0)
        {
            cleanupTimer = cleanupTime;

            if(activePool.Count > 0)
            {
                Cleanup();
            }
        }
    }
}
