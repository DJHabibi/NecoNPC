using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIMovementBehaviour : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public float maxDistance;
    public Animator npcAnimator;
    [SerializeField] public Transform food, entertainment, work;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void Update()
    {
        CheckMoving();
    }
    public void SetWanderDestination()
    {
        Vector3 randomNavMeshPoint = GetRandomNavMeshPoint();
        navMeshAgent.SetDestination(randomNavMeshPoint);
    }
    public void SetNeedsDestination(Vector3 needsPosition)
    {
        navMeshAgent.SetDestination(needsPosition);
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
    public IEnumerator RandomWalk()
    {
        SetWanderDestination();
        while (true)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                SetWanderDestination();
            }
            yield return new WaitForSeconds(Random.Range(5, 20));
        }
    }
    public IEnumerator Eating()
    {
        SetNeedsDestination(GetFoodArea());
        while (true)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                SetNeedsDestination(GetFoodArea());
            }
            yield return new WaitForSeconds(Random.Range(5, 20));
        }
    }
    public IEnumerator Playing()
    {
        SetNeedsDestination(GetPlayArea());
        while (true)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                SetNeedsDestination(GetPlayArea());
            }
            yield return new WaitForSeconds(Random.Range(5, 20));
        }
    }
    public IEnumerator Working()
    {
        SetNeedsDestination(GetWorkArea());
        while (true)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                SetNeedsDestination(GetWorkArea());
            }
            yield return new WaitForSeconds(Random.Range(5, 20));
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
    public Vector3 GetFoodArea()
    {
        Vector3 randomPoint = Vector3.zero;
        Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f, NavMesh.GetAreaFromName("EatingArea")))
        {
            randomPoint = hit.position;
        }
        return randomPoint;
    }
    public Vector3 GetPlayArea()
    {
        Vector3 randomPoint = Vector3.zero;
        Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f, NavMesh.GetAreaFromName("PlayArea")))
        {
            randomPoint = hit.position;
        }
        return randomPoint;
    }
    public Vector3 GetWorkArea()
    {
        Vector3 randomPoint = Vector3.zero;
        Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f, NavMesh.GetAreaFromName("WorkArea")))
        {
            randomPoint = hit.position;
        }
        return randomPoint;
    }
}
