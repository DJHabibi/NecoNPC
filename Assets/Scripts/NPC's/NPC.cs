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
        private float hungerThreshold, boredomThreshold, fullfilmentThreshold;
        public float maxFullfilment;
        public override void Start()
        {
            npcMovement = GetComponent<AIMovementBehaviour>();
            Mathf.Clamp(hunger, 0, Mathf.Infinity);
            Mathf.Clamp(entertained, 0, Mathf.Infinity);
            Mathf.Clamp(fullfilment, 0, maxFullfilment);
            fullfilment = 0;
            hungerThreshold = Random.Range(0, hunger / 5);
            boredomThreshold = Random.Range(0, entertained / 6);
            fullfilmentThreshold = Random.Range(0, maxFullfilment/6);
            Debug.Log(hungerThreshold);
            base.Start();

        }
        public void Update()
        {
            if (Hungry()) 
            {
                npcMovement.StartCoroutine(npcMovement.Eating());
               
                npcMovement.StopCoroutine(npcMovement.RandomWalk());

            }
            if (Bored())
            {
                npcMovement.StartCoroutine(npcMovement.Playing());
                npcMovement.StopCoroutine(npcMovement.RandomWalk());
            }
            if (NotFullfiled())
            {
                npcMovement.StartCoroutine(npcMovement.Working());
                npcMovement.StopCoroutine(npcMovement.RandomWalk());
            }
            if (Hungry() == false || Bored() == false || NotFullfiled() == false)
            {
                npcMovement.StartCoroutine(npcMovement.RandomWalk());
            }


        }
        public bool Hungry()
        {
            hunger -= hungerSpeed * Time.deltaTime;
            if (hunger <= hungerThreshold)
            {
                Debug.Log("Hungry");
                return true;
            }
            else return false;
            
        }
        public bool Bored()
        {
            entertained -= entertainedLoss * Time.deltaTime;
            if (entertained <= boredomThreshold)
            {
                Debug.Log("Bored");
                return true;
               
            }
            else return false;
        }
        public bool NotFullfiled()
        {
            fullfilment -= 1 * Time.deltaTime;
            if (fullfilment <= fullfilmentThreshold)
            {
                Debug.Log(" Not Fullfiled");
                return true;
            }
            else return false;
        }
        public void NPCBehaviour()
        {
            
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

