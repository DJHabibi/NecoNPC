using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIMovementBehaviour : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
   private NPC npc;
   public ChatPrompt prompt;
    public float maxDistance;
    public float areaWidth, areaHeight;
    public Animator npcAnimator;
    public GameObject PILK;
    public GameObject iconEat, iconPlay, iconWork;
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
        npc = GetComponent<NPC>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        PILK.SetActive(false);
        iconEat.SetActive(false);
        iconPlay.SetActive(false);
        iconWork.SetActive(false);
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
        npc.UpdateState("STATUS UPDATE: YOU ARE NOW walking: context: you are walking around");
        yield return new WaitForSeconds(Random.Range(5, 15));
        isWanderingCoroutineRunning = false;
    }
    public IEnumerator Eating()
    {
        isEatingCoroutineRunning = true;
        iconEat.SetActive(isEatingCoroutineRunning);
        Vector3 Area = new Vector3(Random.Range(food.position.x-areaWidth, food.position.x+areaWidth),food.position.y, Random.Range(food.position.z - areaHeight, food.position.z + areaHeight));
        SetNeedsDestination(Area);

        while (Vector3.Distance(transform.position, Area) > 1)
        {
            yield return null; // Wait until the NPC is close to the food position
        }

        isEating = true;
        npcAnimator.SetBool("Eating", true);
        PILK.SetActive(true);
        npc.UpdateState("STATUS UPDATE: YOU ARE NOW eating: context: you are drinking pilk, pilk is the combination of pepsi and milk");
        yield return new WaitForSeconds(25);
        npc.UpdateState( "STATUS UPDATE: YOU ARE NOW not eating: context: you are done drinking pilk");
        isEatingCoroutineRunning = false;
        isEating = false;
        iconEat.SetActive(isEatingCoroutineRunning);
        PILK.SetActive(false);
        npcAnimator.SetBool("Eating", false);
    }

    public IEnumerator Playing()
    {
        isPlayingCoroutineRunning = true;
        iconPlay.SetActive(isPlayingCoroutineRunning);
        Vector3 Area = new Vector3(Random.Range(entertainment.position.x - areaWidth, entertainment.position.x + areaWidth), entertainment.position.y, Random.Range(entertainment.position.z - areaHeight, entertainment.position.z + areaHeight));
        SetNeedsDestination(Area);

        while (Vector3.Distance(transform.position, Area) > 1f)
        {
            yield return null; // Wait until the NPC is close to the entertainment position
        }

        isPlaying = true;
        npcAnimator.SetBool("Playing", true);
        npc.UpdateState("STATUS UPDATE: YOU ARE NOW playing: context: you are dancing gangnam style");
        yield return new WaitForSeconds(25);
        npc.UpdateState("STATUS UPDATE: YOU ARE NOW not playing: context: you finished dancing gangnam style");
        npcAnimator.SetBool("Playing", false);
        isPlayingCoroutineRunning = false;
        iconPlay.SetActive(isPlayingCoroutineRunning);
        isPlaying = false;
    }

    public IEnumerator Working()
    {
        isWorkingCoroutineRunning = true;
        iconWork.SetActive(isWorkingCoroutineRunning);
       Vector3 Area = new Vector3(Random.Range(work.position.x - areaWidth, work.position.x + areaWidth), work.position.y, Random.Range(work.position.z - areaHeight, work.position.z + areaHeight));
        SetNeedsDestination(Area);

        while (Vector3.Distance(transform.position, Area) > 1f)
        {
            yield return null; // Wait until the NPC is close to the work position
        }

        isWorking = true;
        npcAnimator.SetBool("Working", true);
        npc.UpdateState( "STATUS UPDATE: YOU ARE NOW working: context: you are training and practicing shadow boxing");
        yield return new WaitForSeconds(25);
        npc.UpdateState( "STATUS UPDATE: YOU ARE NOW not working: context: you are done training and practicing shadow boxing");
        isWorkingCoroutineRunning = false;
        iconWork.SetActive(isWorkingCoroutineRunning);
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

}