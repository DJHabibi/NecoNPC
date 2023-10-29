using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenAI
{
    public class BoredNode : Node
    {
        private NPC npc;
        private AIMovementBehaviour aIMovement;
        private float threshold;

        public BoredNode(NPC npc, float threshold, AIMovementBehaviour aIMovement)
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
                Debug.LogError("NPC or AIMovementBehaviour is null in BoredNode.");
                return NodeState.FAILURE;
            }

            if (npc.entertained <= threshold &&
                !aIMovement.isEatingCoroutineRunning &&
                 !aIMovement.isChattingCoroutineRunning &&
                !aIMovement.isWorkingCoroutineRunning &&
                !aIMovement.isPlayingCoroutineRunning)
            {
                aIMovement.StopCoroutine(aIMovement.RandomWalk());
                aIMovement.StartCoroutine(aIMovement.Playing());
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
    }
}
