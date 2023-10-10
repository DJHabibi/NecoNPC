using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenAI
{
    public class WorkNode : Node
    {
        private NPC npc;
        private AIMovementBehaviour aIMovement;
        private float threshold;
        public WorkNode(NPC npc, float threshold)
        {
            this.npc = npc;
            this.npc.fullfilmentThreshold = threshold;
        }
        // Start is called before the first frame update
        public override NodeState Evaluate()
        {
            return npc.fullfilment <= threshold && !aIMovement.isEatingCoroutineRunning && !aIMovement.isWorkingCoroutineRunning && !aIMovement.isEatingCoroutineRunning ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}

