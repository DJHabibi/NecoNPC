using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.AI;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;
        private float height;
        private OpenAIApi openai = new OpenAIApi();
        private List<ChatMessage> messages = new List<ChatMessage>();

        GameObject player;
        [SerializeField] public ChatPrompt npcBehaviour;
        [SerializeField] Animator animator;
        [SerializeField] GameObject message;
        [SerializeField] NavMeshAgent agent;
        [SerializeField] private CameraManager camera;
        private float agentDefaultSpeed;
        private bool isPlayerInsideTrigger;

        public virtual void Start()
        {
            message.SetActive(false);
            player = FindObjectOfType<PlayerMovement>().gameObject;
            agent = GetComponent<NavMeshAgent>();
            agentDefaultSpeed = agent.speed;
            camera = FindObjectOfType<CameraManager>();
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        public async void SendReply()
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };

            AppendMessage(newMessage);

            if (messages.Count == 0) newMessage.Content = npcBehaviour.NpcBehaviour + "\n" + inputField.text;

            messages.Add(newMessage);

            button.enabled = false;
            inputField.text = "";

            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                messages.Add(message);
                AppendMessage(message);
                animator.SetTrigger("Talk");
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
        }
        public async void UpdateState(string state)
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = state,
            };

            AppendMessage(newMessage);

            messages.Add(newMessage);

            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });
        }
        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player)
            {
                isPlayerInsideTrigger = true;
                message.SetActive(true);
                inputField = GameObject.FindGameObjectWithTag("PlayerInputField").GetComponent<InputField>();
                button = GameObject.FindGameObjectWithTag("PlayerButton").GetComponent<Button>();
                button.onClick.AddListener(SendReply);
                agent.speed = 0;
               
            }
        }
        public virtual void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                isPlayerInsideTrigger = false;
                message.SetActive(false);
                inputField = null;
                button.onClick.RemoveListener(SendReply);
                button = null;
                agent.speed = agentDefaultSpeed;
               

            }
        }
        public virtual void OnTriggerStay(Collider other)
        {
            if(other.gameObject == player && inputField.isFocused)
            {
                camera.focusTransform = this.gameObject.transform;
            }
            if (inputField !=null && !inputField.isFocused)
            {
                camera.focusTransform = camera.playerFocus;
            }
            if (isPlayerInsideTrigger && inputField != null && inputField.isFocused)
            {
                // Continuously rotate to face the player when the player is inside and input is focused
                RotateToFacePlayer(player.transform);
            }
        }
        private void RotateToFacePlayer(Transform target)
        {
            Vector3 lookDirection = target.position - transform.position;
            lookDirection.y = 0; 

            if (lookDirection != Vector3.zero)
            {
                //Rotate the object to face the player
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 2);
            }
        }
    }
}
