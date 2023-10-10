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
        public override void Start()
        {
            baseHungerSpeed = hungerSpeed;
            baseentertainedLossSpeed = entertainedLoss;
            baseFullfilmentLossSpeed = fullfilmentLoss;

            npcMovement = GetComponent<AIMovementBehaviour>();
            hungerThreshold = Random.Range(0, hunger / 4);
            boredomThreshold = Random.Range(0, entertained / 5);
            fullfilmentThreshold = Random.Range(0, maxFullfilment / 6);
            base.Start();

        }
        public void Update()
        {
            if (Hungry() && !npcMovement.isPlayingCoroutineRunning && !npcMovement.isWorkingCoroutineRunning && !npcMovement.isEatingCoroutineRunning)
            {

                npcMovement.StartCoroutine(npcMovement.Eating());

            }
            if (Bored() && !npcMovement.isPlayingCoroutineRunning && !npcMovement.isWorkingCoroutineRunning && !npcMovement.isEatingCoroutineRunning)
            {
                npcMovement.StartCoroutine(npcMovement.Playing());

            }
            if (NotFullfiled() && !npcMovement.isPlayingCoroutineRunning && !npcMovement.isWorkingCoroutineRunning && !npcMovement.isEatingCoroutineRunning)
            {
                npcMovement.StartCoroutine(npcMovement.Working());

            }
            if (!Hungry() && !Bored() && !NotFullfiled() && !npcMovement.isWanderingCoroutineRunning && !npcMovement.isPlayingCoroutineRunning && !npcMovement.isWorkingCoroutineRunning && !npcMovement.isEatingCoroutineRunning)
            {
                npcMovement.StartCoroutine(npcMovement.RandomWalk());
            }
            else StopCoroutine(npcMovement.RandomWalk());


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

