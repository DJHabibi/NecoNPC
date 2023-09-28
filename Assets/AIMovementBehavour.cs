using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIMovementBehavour : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public float maxDistance;

    private void Start()
    {
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetRandomDestination();
        StartCoroutine(RandomWalk());
    }

    private void SetRandomDestination()
    {
        Vector3 randomNavMeshPoint = GetRandomNavMeshPoint();
        navMeshAgent.SetDestination(randomNavMeshPoint);
    }

    private Vector3 GetRandomNavMeshPoint()
    {
        Vector3 randomPoint = Vector3.zero;
        Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f, NavMesh.AllAreas))
        {
            randomPoint = hit.position;
        }
        return randomPoint;
    }
    private IEnumerator RandomWalk()
    {
        while (true)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                SetRandomDestination();
            }
            yield return new WaitForSeconds(10.0f);
        }
    }
}
