using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace OpenAI
{
    public class NPC : ChatGPT
    {
        private AIMovementBehaviour npcMovement;
        [SerializeField] public float hunger;
        [SerializeField] public float entertained;
        [SerializeField] public float fullfilment;
        [SerializeField] public float hungerSpeed;
        [SerializeField] public float entertainedLoss;
        [SerializeField] public float fullfilmentLoss;
        public float hungerThreshold, boredomThreshold, fullfilmentThreshold;
        private float baseHungerSpeed, baseentertainedLossSpeed, baseFullfilmentLossSpeed;
        public float maxFullfilment;

        private Node topNode;
        public override void Start()
        {
            npcMovement = GetComponent<AIMovementBehaviour>();
            ConstructBehaviourTree();
            baseHungerSpeed = hungerSpeed;
            baseentertainedLossSpeed = entertainedLoss;
            baseFullfilmentLossSpeed = fullfilmentLoss;

            hungerThreshold = Random.Range(0, hunger / 4);
            boredomThreshold = Random.Range(0, entertained / 5);
            fullfilmentThreshold = Random.Range(0, maxFullfilment / 6);
            base.Start();

        }

        private void ConstructBehaviourTree()
        {
            HungerNode hungerNode = new HungerNode(this, hungerThreshold,npcMovement);
            BoredNode boredNode = new BoredNode(this, boredomThreshold, npcMovement);
            WorkNode workNode = new WorkNode(this, fullfilmentThreshold,npcMovement);
            WanderNode wanderNode = new WanderNode(this, npcMovement);

            topNode = new Selector(new List<Node> { hungerNode, boredNode, workNode, wanderNode });
        }

        public void Update()
        {
            Hungry();
            Bored();
            NotFullfiled();
            topNode.Evaluate();
        }
        public bool Hungry()
        {
            if (!npcMovement.isEating)
            {
                hunger -= hungerSpeed * Time.deltaTime;
            }
            else hunger += hungerSpeed * 4 * Time.deltaTime;
            if (hunger < 0)
            {
                hunger = 0;
            }
            if (hunger <= hungerThreshold)
            {
                return true;
            }
            else return false;

        }
        public bool Bored()
        {
            if (!npcMovement.isPlaying)
            {
                entertained -= entertainedLoss * Time.deltaTime;
            }
            else entertained += entertainedLoss * 4 * Time.deltaTime;
            if (entertained < 0)
            {
                entertained = 0;
            }
            if (entertained <= boredomThreshold)
            {
                return true;
            }
            else return false;
        }
        public bool NotFullfiled()
        {
            if (!npcMovement.isWorking)
            {
                fullfilment -= fullfilmentLoss * Time.deltaTime;
            }
            else fullfilment += fullfilmentLoss * 4 * Time.deltaTime;
            if (fullfilment < 0)
            {
                fullfilment = 0;
            }
            if (fullfilment <= fullfilmentThreshold)
            {
                return true;
            }
            else return false;
        }
        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }
        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
        }
        public override void OnTriggerStay(Collider other)
        {
            base.OnTriggerStay(other);
        }
    }
}

