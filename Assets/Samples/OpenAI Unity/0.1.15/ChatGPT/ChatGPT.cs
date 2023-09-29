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
        [SerializeField] GameObject canvas;
        [SerializeField] NavMeshAgent agent;
        [SerializeField] private CameraManager camera;
        private float agentDefaultSpeed;
        private void Start()
        {
            canvas.SetActive(false);
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

        private async void SendReply()
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
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player)
            {
                canvas.SetActive(true);
                inputField = GameObject.FindGameObjectWithTag("PlayerInputField").GetComponent<InputField>();
                button = GameObject.FindGameObjectWithTag("PlayerButton").GetComponent<Button>();
                button.onClick.AddListener(SendReply);
                agent.speed = 0;
               
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                canvas.SetActive(false);
                inputField = null;
                button.onClick.RemoveListener(SendReply);
                button = null;
                agent.speed = agentDefaultSpeed;
               

            }
        }
        public void OnTriggerStay(Collider other)
        {
            if(other.gameObject == player && inputField.isFocused)
            {
                camera.focusTransform = this.gameObject.transform;
            }
            if (inputField !=null && !inputField.isFocused)
            {
                camera.focusTransform = camera.playerFocus;
            }
        }
    }
}
