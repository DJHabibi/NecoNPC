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
    public Animator npcAnimator;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
       
        StartCoroutine(RandomWalk());
    }
    public void Update()
    {
        CheckMoving();
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
        SetRandomDestination();
        while (true)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                SetRandomDestination();
            }

            // Check if the agent's velocity magnitude is greater than a small threshold.     
            yield return new WaitForSeconds(Random.Range(5,20)); // Yielding once per frame.
        }
    }
    private void CheckMoving()
    {
        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            npcAnimator.SetBool("Walking", true);
        }
        else
        {
            npcAnimator.SetBool("Walking", false);
        }
    }
}
