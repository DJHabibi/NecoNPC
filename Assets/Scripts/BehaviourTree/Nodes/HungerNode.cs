using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenAI
{
    public class HungerNode : Node
    {
        private NPC npc;
        private AIMovementBehaviour aIMovement;
        private float threshold;

        public HungerNode(NPC npc, float threshold, AIMovementBehaviour aIMovement)
        {
            this.npc = npc;
            this.threshold = threshold;
            this.aIMovement = aIMovement;
        }

        // Start is called before the first frame update
        public override NodeState Evaluate()
        {
            if (npc == null)
            {
                Debug.LogError("NPC is null in HungerNode.");
                return NodeState.FAILURE;
            }

            // Check for hunger, coroutine flags, and return appropriate state.
            if (npc.hunger <= threshold &&
                !aIMovement.isEatingCoroutineRunning &&
                !aIMovement.isWorkingCoroutineRunning &&
                !aIMovement.isPlayingCoroutineRunning)
            {
                Debug.Log("HungerNode: SUCCESS");
                aIMovement.StartCoroutine(aIMovement.Eating());
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
    }
}
