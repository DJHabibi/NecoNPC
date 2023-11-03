using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIMovementBehaviour : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public AIMovementBehaviour npcMovement;
    private NPC npc;
    public NPC npc2;
    public float maxDistance;
    public float areaWidth, areaHeight;
    public Animator npcAnimator, npc2Animator;

    public Transform npc1;
    public GameObject iconEat, iconPlay, iconWork,iconChat;
    public GameObject PILK;

    [SerializeField] public Transform food, entertainment, work;

    public bool isEatingCoroutineRunning = false;
    public bool isPlayingCoroutineRunning = false;
    public bool isWorkingCoroutineRunning = false;
    public bool isWanderingCoroutineRunning = false;
    public bool isChattingCoroutineRunning;

    public bool isPlaying;
    public bool isWorking;
    public bool isEating;
    public bool isChatting;
    public bool closeToTalk;


    private void Start()
    {
        npc = GetComponent<NPC>();
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        PILK.SetActive(false);
        iconEat.SetActive(false);
        iconPlay.SetActive(false);
        iconWork.SetActive(false);
        iconChat.SetActive(false);
    }
    public void Update()
    {
        CheckMoving();
        CheckIfCloseToTalk();
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
        npc.UpdateState("STATUS UPDATE: you are walking: context: you are walking around");
        yield return new WaitForSeconds(Random.Range(5, 15));
        isWanderingCoroutineRunning = false;
    }
    public IEnumerator Eating()
    {
        isEatingCoroutineRunning = true;
        iconEat.SetActive(isEatingCoroutineRunning);
        Vector3 Area = new Vector3(Random.Range(food.position.x - areaWidth, food.position.x + areaWidth), food.position.y, Random.Range(food.position.z - areaHeight, food.position.z + areaHeight));
        SetNeedsDestination(Area);

        while (Vector3.Distance(transform.position, Area) > 1)
        {
            yield return null; // Wait until the NPC is close to the food position
        }

        isEating = true;
        npcAnimator.SetBool("Eating", true);
        PILK.SetActive(true);
        npc.UpdateState("STATUS UPDATE: you are eating: context: you are drinking pilk, pilk is the combination of pepsi and milk");
        yield return new WaitForSeconds(25);
        npc.UpdateState("STATUS UPDATE: you are done eating: context: you are done drinking pilk");
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
        npc.UpdateState("STATUS UPDATE: you are dancing: context: you are dancing gangnam style");
        yield return new WaitForSeconds(25);
        npc.UpdateState("STATUS UPDATE: you are done dancing: context: you finished dancing gangnam style");
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
        npc.UpdateState("STATUS UPDATE: you are working: context: you are training and practicing shadow boxing");
        yield return new WaitForSeconds(25);
        npc.UpdateState("STATUS UPDATE: you are not working: context: you are done training and practicing shadow boxing");
        isWorkingCoroutineRunning = false;
        iconWork.SetActive(isWorkingCoroutineRunning);
        isWorking = false;
        npcAnimator.SetBool("Working", false);
    }
    public IEnumerator Chatting()
    {
        isChattingCoroutineRunning = true;
        iconChat.SetActive(true);
        npcMovement.StopCoroutine(RandomWalk());
        npcMovement.navMeshAgent.speed = 0;
        while (Vector3.Distance(transform.position, npc1.position) > 1f)
        {
            // Calculate the destination based on the position of npc1
            closeToTalk = false;

            yield return null;
        }
        closeToTalk = true;

        // Start the conversation with the chat message
        isChatting = true;
        #region Dialog

        var npcResponse1Task = npc.InitialChat(npc,"Hello");
        npcAnimator.SetTrigger("Talk");
        // Yield until the task is complete
        while (!npcResponse1Task.IsCompleted)
        {
            yield return null;
        }

        string npcResponse1 = npcResponse1Task.Result;
        Debug.Log(npcResponse1);
        yield return new WaitForSeconds(3);

        var npcResponse2Task = npc2.InitialChat(npc2,npcResponse1);
        npc2Animator.SetTrigger("Talk");
        // Yield until the task is complete
        while (!npcResponse2Task.IsCompleted)
        {
            yield return null;
        }
        string npcResponse2 = npcResponse2Task.Result;
        Debug.Log(npcResponse2);
        yield return new WaitForSeconds(3);

        var npcResponse3Task = npc.InitialChat(npc,npcResponse2);
        npcAnimator.SetTrigger("Talk");
        // Yield until the task is complete
        while (!npcResponse3Task.IsCompleted)
        {
            yield return null;
        }

        string npcResponse3 = npcResponse3Task.Result;
        Debug.Log(npcResponse3);
        yield return new WaitForSeconds(3);

        var npcResponse4Task = npc2.InitialChat(npc2,npcResponse3);
        npc2Animator.SetTrigger("Talk");
        // Yield until the task is complete
        while (!npcResponse4Task.IsCompleted)
        {
            yield return null;
        }
        string npcResponse4 = npcResponse4Task.Result;
        Debug.Log(npcResponse4);
        yield return new WaitForSeconds(3);

        var npcResponse5Task = npc.InitialChat(npc, npcResponse4);
        npcAnimator.SetTrigger("Talk");
        // Yield until the task is complete
        while (!npcResponse5Task.IsCompleted)
        {
            yield return null;
        }
        string npcResponse5 = npcResponse5Task.Result;
        Debug.Log(npcResponse5);
        yield return new WaitForSeconds(3);

        var npcResponse6Task = npc2.InitialChat(npc2, npcResponse5);
        npc2Animator.SetTrigger("Talk");
        // Yield until the task is complete
        while (!npcResponse6Task.IsCompleted)
        {
            yield return null;
        }
        string npcResponse6 = npcResponse6Task.Result;
        Debug.Log(npcResponse6);
        yield return new WaitForSeconds(3);

        #endregion
        // Continue chatting for 25 seconds, you can add more messages here
        yield return new WaitForSeconds(25);

        // After chatting, reset isChatting to false and allow the NPC to move again
        isChatting = false;
        // Set isChattingCoroutineRunning to false to indicate the end of the conversation
        isChattingCoroutineRunning = false;
        iconChat.SetActive(false);
        npcMovement.navMeshAgent.speed = 2;
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
    private void CheckIfCloseToTalk()
    {
        Vector3 area;
        if (!closeToTalk && isChattingCoroutineRunning)
        {
            area = new Vector3(Random.Range(npc1.position.x - 0.5f, npc1.position.x + 0.5f), npc1.position.y, Random.Range(npc1.position.z - 0.5f, npc1.position.z + 0.5f));
            // Set the destination to the calculated area
            SetNeedsDestination(area);
        }
        if(closeToTalk && isChattingCoroutineRunning)
        {
            npc.RotateToFacePlayer(npc1.transform,this.transform);
            
        }
    }
}