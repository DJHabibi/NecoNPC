using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenAI
{
    public class ChatNode : Node
    {
        private NPC npc;
        private AIMovementBehaviour aIMovement;
        private float threshold;

        public ChatNode(NPC npc, float threshold, AIMovementBehaviour aIMovement)
        {
            this.npc = npc;
            this.threshold = threshold;
            this.aIMovement = aIMovement;
        }

        // Start is called before the first frame update
        public override NodeState Evaluate()
        {
            if (npc == null || aIMovement == null)
            {
                Debug.LogError("NPC or AIMovementBehaviour is null in WorkNode.");
                return NodeState.FAILURE;
            }
/*
            Debug.Log("isEatingCoroutineRunning: " + aIMovement.isEatingCoroutineRunning);
            Debug.Log("isWorkingCoroutineRunning: " + aIMovement.isWorkingCoroutineRunning);
            Debug.Log("isPlayingCoroutineRunning: " + aIMovement.isPlayingCoroutineRunning);*/

            // Check for fulfillment, coroutine flags, and return appropriate state.
            if (npc.socialNeed <= threshold &&
                !aIMovement.isEatingCoroutineRunning &&
                 !aIMovement.isChattingCoroutineRunning &&
                !aIMovement.isWorkingCoroutineRunning &&
                !aIMovement.isPlayingCoroutineRunning)
            {
                Debug.Log("ChatNode: SUCCESS");
                aIMovement.StopCoroutine(aIMovement.RandomWalk());
                aIMovement.StartCoroutine(aIMovement.Chatting());
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
    }
}
