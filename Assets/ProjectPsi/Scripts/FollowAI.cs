using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum States
{
    FOLLOWING,//follow player until certain distance is reached, then goto shooting
    SEARCHING,//stand still and wait for about 5 seconds to aquire line of sight. if aquired goto follow else goto patroll
    PATROLLING,//go to patrol route and goto points one by one. if line of sight is aquired, goto follow
    SHOOTING, //shoot at player until either player exits shooting range, or leaves line of sight
}

public class FollowAI : MonoBehaviour
{
    public LayerMask obstacles;
    public LayerMask navMesh;
    public float shootingDistance = 5;
    public float sightDistance = 10;
    public int health = 50;
    public int armor = 2;

    public GameObject bulletPrefab;
    public Transform gun;

    private NavMeshAgent agent;
    private Transform player;
    private States state = States.SEARCHING;
    private List<GameObject> bullets = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GlobalVars.Get("head").transform;
        InvokeRepeating("Cleanup", 3, 3);
    }

    float t = 0;
    bool hasTarget = false;
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case States.SEARCHING:
                {
                    
                    if (CheckLineOfSight())
                    {
                        if (GetDistance() > shootingDistance)
                        {
                            state = States.FOLLOWING;
                        }
                        else
                        {
                            state = States.SHOOTING;
                        }
                    }
                    //go to patrol here
                }
                break;
            case States.FOLLOWING:
                {
                    //Debug.Log("following");
                    if (CheckLineOfSight())
                    {
                        if(GetDistance() < shootingDistance)
                        {
                            state = States.SHOOTING;
                        }
                        else
                        {
                            agent.destination = GetPointOnNavMesh(player.position);
                        }
                    }
                    else state = States.SEARCHING;
                }
                break;
            case States.PATROLLING:
                {
                    agent.isStopped = true;
                }
                break;
            case States.SHOOTING:
                {
                    agent.isStopped = true;
                    //Debug.Log("shooting");
                    if (CheckLineOfSight())
                    {
                        if(GetDistance() > shootingDistance)
                        {
                            agent.isStopped = false;
                            state = States.FOLLOWING;
                        }
                        else
                        {
                            if(t <= 0)
                            {
                                Debug.Log("shooting");
                                Vector3 heading = player.position - (transform.position + Vector3.up);

                                GameObject b = Instantiate(bulletPrefab, gun.position, Quaternion.identity);

                                b.GetComponent<Rigidbody>().AddForce(heading.normalized * 40, ForceMode.Impulse);
                                bullets.Add(b);
                                t = 3;
                            }
                        }
                    }
                    else
                    {
                        agent.isStopped = false;
                        state = States.SEARCHING;
                    }
                }
                break;
        }

        if (t > 0) t -= Time.deltaTime;
    }

    bool CheckLineOfSight()
    {
        RaycastHit hit;
        float distance = Vector3.Distance(player.position, transform.position);

        Vector3 heading = player.position - transform.position;
        Vector3 direction = heading / distance;

        return !Physics.Raycast(transform.position, direction, out hit, distance, obstacles) && GetDistance() <= sightDistance;
    }

    float GetDistance()
    {
        return Vector3.Distance(player.position, transform.position);
    }

    Vector3 GetPointOnNavMesh(Vector3 pos)
    {
        RaycastHit hit;
        if(Physics.Raycast(pos, Vector3.down, out hit, 10, navMesh))
        {
            
            return hit.point;
        }
        Debug.LogWarning("PLAYER IS NOT ON NAVMESH");
        return player.position;
    }

    private void Cleanup()
    {
        if (bullets.Count > 0)
        {
            Destroy(bullets[0]);
            bullets.Remove(bullets[0]);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody b = collision.collider.GetComponent<Rigidbody>();
        if (!collision.collider.CompareTag("projectile"))//dont hit self with own bullets
        {
            if(b != null)
            {
                //float force = collision.impulse.magnitude;

                float force = b.velocity.magnitude;
                //Debug.Log(force);
                int damage = Mathf.FloorToInt(force);
                if(damage > armor)health -= damage;
            }
            else
            {
                Debug.Log("body null");
            }
            //Debug.Log("impact of " + (int)(force / 100) + " detected by turret, health at " + health);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    switch (state)
    //    {
    //        case States.SEARCHING:
    //        {
    //                Gizmos.color = Color.blue;
    //                Gizmos.DrawSphere(transform.position, 1);
    //        }break;
    //        case States.FOLLOWING:
    //            {
    //                Gizmos.color = Color.yellow;
    //                Gizmos.DrawSphere(transform.position, 1);
    //            }
    //            break;
    //        case States.SHOOTING:
    //            {
    //                Gizmos.color = Color.red;
    //                Gizmos.DrawSphere(transform.position, 1);
    //            }
    //            break;
    //        case States.PATROLLING:
    //            {
    //                Gizmos.color = Color.green;
    //                Gizmos.DrawSphere(transform.position, 1);
    //            }
    //            break;
    //    }
    //}
}
