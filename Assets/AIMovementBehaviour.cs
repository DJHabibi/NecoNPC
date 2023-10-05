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
    public float areaWidth, areaHeight;
    public Animator npcAnimator;
    public GameObject PILK;
    public bool isEatingCoroutineRunning = false;
    public bool isPlayingCoroutineRunning = false;
    public bool isWorkingCoroutineRunning = false;
    public bool isWanderingCoroutineRunning = false;
    [SerializeField] public Transform food, entertainment, work;
    public bool isPlaying;
    public bool isWorking;
    public bool isEating;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        PILK.SetActive(false);
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
    public void SetNeedsDestination(Vector3 needs)
    {
        navMeshAgent.SetDestination(needs);
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
        yield return new WaitForSeconds(Random.Range(5, 15));
        isWanderingCoroutineRunning = false;
    }
    public IEnumerator Eating()
    {
        isEatingCoroutineRunning = true;
        Vector3 Area = new Vector3(Random.Range(food.position.x-areaWidth, food.position.x+areaWidth),food.position.y, Random.Range(food.position.z - areaHeight, food.position.z + areaHeight));
        SetNeedsDestination(Area);

        while (Vector3.Distance(transform.position, Area) > 1)
        {
            yield return null; // Wait until the NPC is close to the food position
        }

        isEating = true;
        npcAnimator.SetBool("Eating", true);
        PILK.SetActive(true);
        yield return new WaitForSeconds(25);
        isEatingCoroutineRunning = false;
        isEating = false;
        PILK.SetActive(false);
        npcAnimator.SetBool("Eating", false);
    }

    public IEnumerator Playing()
    {
        isPlayingCoroutineRunning = true;
        Vector3 Area = new Vector3(Random.Range(entertainment.position.x - areaWidth, entertainment.position.x + areaWidth), entertainment.position.y, Random.Range(entertainment.position.z - areaHeight, entertainment.position.z + areaHeight));
        SetNeedsDestination(Area);

        while (Vector3.Distance(transform.position, Area) > 1f)
        {
            yield return null; // Wait until the NPC is close to the entertainment position
        }

        isPlaying = true;
        npcAnimator.SetBool("Playing", true);
        yield return new WaitForSeconds(25);
        npcAnimator.SetBool("Playing", false);
        isPlayingCoroutineRunning = false;
        isPlaying = false;
    }

    public IEnumerator Working()
    {
        isWorkingCoroutineRunning = true;
        Vector3 Area = new Vector3(Random.Range(work.position.x - areaWidth, work.position.x + areaWidth), work.position.y, Random.Range(work.position.z - areaHeight, work.position.z + areaHeight));
        SetNeedsDestination(Area);

        while (Vector3.Distance(transform.position, Area) > 1f)
        {
            yield return null; // Wait until the NPC is close to the work position
        }

        isWorking = true;
        npcAnimator.SetBool("Working", true);
        yield return new WaitForSeconds(25);
 
        isWorkingCoroutineRunning = false;
        isWorking = false;
        npcAnimator.SetBool("Working", false);
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
    //I tried to be cool, but navmesh didnt wanted to get a random area in the index i especified, so i was forced to be normal and use a transform :(
    /* public Vector3 GetFoodArea()
     {
         Vector3 randomPoint = Vector3.zero;
         Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
         randomDirection += transform.position;

         NavMeshHit hit;
         if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f,1))
         {
             Debug.Log(NavMesh.GetAreaFromName("Eating Area"));
             randomPoint = hit.position;
             Debug.Log(randomPoint);
         }
         return randomPoint;
     }
     public Vector3 GetPlayArea()
     {
         Vector3 randomPoint = Vector3.zero;
         Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
         randomDirection += transform.position;

         NavMeshHit hit;
         if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f, 4))
         {
             Debug.Log(NavMesh.GetAreaFromName("Play Area"));
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
         if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f, 5))
         {
             Debug.Log(NavMesh.GetAreaFromName("Work Area"));
             randomPoint = hit.position;
         }
         return randomPoint;
     }*/
}