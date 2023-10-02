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
    public bool isEatingCoroutineRunning = false;
    public bool isPlayingCoroutineRunning = false;
    public bool isWorkingCoroutineRunning = false;
    public bool isWanderingCoroutineRunning = false;
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
        isWanderingCoroutineRunning = true;
        SetWanderDestination();
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            SetWanderDestination();
        }
        yield return new WaitForSeconds(Random.Range(5, 20));
        isWanderingCoroutineRunning = false;

    }
    public IEnumerator Eating()
    {
        isEatingCoroutineRunning = true;
        SetNeedsDestination(GetFoodArea());
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            SetNeedsDestination(GetFoodArea());
        }
        yield return new WaitForSeconds(Random.Range(5, 20));
        isEatingCoroutineRunning = false;

    }
    public IEnumerator Playing()
    {
        isPlayingCoroutineRunning = true;
        SetNeedsDestination(GetPlayArea());

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            SetNeedsDestination(GetPlayArea());
        }
        yield return new WaitForSeconds(Random.Range(5, 20));
        isPlayingCoroutineRunning = false;

    }
    public IEnumerator Working()
    {
        isWorkingCoroutineRunning = true;
        SetNeedsDestination(GetWorkArea());

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            SetNeedsDestination(GetWorkArea());
        }
        yield return new WaitForSeconds(Random.Range(5, 20));
        isWorkingCoroutineRunning = true;

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
        if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f, NavMesh.GetAreaFromName("Eating Area")))
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
        if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f, NavMesh.GetAreaFromName("Play Area")))
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
        if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f, NavMesh.GetAreaFromName("Work Area")))
        {
            randomPoint = hit.position;
        }
        return randomPoint;
    }
}