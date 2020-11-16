﻿using System.Collections;
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

    private NavMeshAgent agent;
    private Transform player;
    private States state = States.SEARCHING;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GlobalVars.Get("head").transform;
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
                    Debug.Log("searching");
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
                    Debug.Log("following");
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

                }
                break;
            case States.SHOOTING:
                {
                    agent.isStopped = true;
                    Debug.Log("shooting");
                    if (CheckLineOfSight())
                    {
                        if(GetDistance() > shootingDistance)
                        {
                            agent.isStopped = false;
                            state = States.FOLLOWING;
                        }
                        else
                        {
                            Debug.Log("shooting");
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

        //if (t > 0) t -= Time.deltaTime;
    }

    bool CheckLineOfSight()
    {
        RaycastHit hit;
        float distance = Vector3.Distance(player.position, transform.position);

        Vector3 heading = player.position - transform.position;
        Vector3 direction = heading / distance;

        return !Physics.Raycast(transform.position, direction, out hit, distance, obstacles);
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
}
