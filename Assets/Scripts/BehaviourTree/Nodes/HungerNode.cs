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
        public HungerNode(NPC npc, float threshold)
        {
            this.npc = npc;
            this.npc.hungerThreshold = threshold; 
        }
    // Start is called before the first frame update
    public override NodeState Evaluate()
        {
            return npc.hunger  <= threshold && !aIMovement.isEatingCoroutineRunning && !aIMovement.isWorkingCoroutineRunning && !aIMovement.isEatingCoroutineRunning ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}

