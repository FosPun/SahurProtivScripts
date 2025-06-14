using System;
using UnityEngine;
using UnityEngine.AI;



public class ClaudyBehaviour : EnemyBehaviour
{
    [SerializeField] private Transform[] waypoints;
    private int currentWaypoint;
    private Vector3 target;
    private float distanceToPlayer;
    [SerializeField] private int distanceForScatter;
    [SerializeField] private int coinsForStart;
    
    private void FixedUpdate()
    {
        CalculateDistanceToPlayer();
    }

    private void UpdateDestination()
    {
        if (distanceToPlayer > distanceForScatter)
        {
            if (!(Vector3.Distance(transform.position, target) < 1)) return;
            target = targetPlayer.position;

        }
        else
        {
            Patroling();
        }
    }
    

    private void CalculateDistanceToPlayer()
    {
        if (targetPlayer)
        {
            distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);
        }
    }

    private void IterateWaypointIndex()
    {
        currentWaypoint++;
        if (currentWaypoint == waypoints.Length)
        {
            currentWaypoint = 0;
        }
    }
    

    private void Patroling()
    {
        target = waypoints[currentWaypoint].position;
        if (!(Vector3.Distance(transform.position, target) < 1)) return;
        IterateWaypointIndex();
    }
    
    protected override void MovementEnemy()
    {
        agent.SetDestination(target);
        UpdateDestination();
    }

    protected override bool CanAct()
    {
        return gameManager.coinsPicked >= coinsForStart;
    }
}
