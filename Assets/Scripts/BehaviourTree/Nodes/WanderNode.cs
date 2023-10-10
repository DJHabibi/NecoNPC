using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenAI
{
    public class WanderNode : Node
    {
        private NPC npc;
        private AIMovementBehaviour aIMovement;
        public override NodeState Evaluate()
        {
            // Check for hunger, boredom, and fullfilment, in addition to coroutine flags.
            if (!aIMovement.isWanderingCoroutineRunning &&
                !aIMovement.isEatingCoroutineRunning &&
                !aIMovement.isWorkingCoroutineRunning &&
                !aIMovement.isEatingCoroutineRunning &&
                !npc.Hungry() && // Check for hunger
                !npc.Bored() &&  // Check for boredom
                !npc.NotFullfiled()) // Check for fullfilment
            {
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
    }
}

