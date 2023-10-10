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
        public BoredNode(NPC npc, float threshold)
        {
            this.npc = npc;
            this.npc.boredomThreshold = threshold;
        }
        // Start is called before the first frame update
        public override NodeState Evaluate()
        {
            return npc.entertained <= threshold && !aIMovement.isEatingCoroutineRunning && !aIMovement.isWorkingCoroutineRunning && !aIMovement.isEatingCoroutineRunning ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}

