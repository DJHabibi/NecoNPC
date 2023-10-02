using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ChatInputs : MonoBehaviour
{
    private InputField chatField;
    public Button chatButton;
    public PlayerMovement playerMovement;
    public CameraManager cameraManager;
    PlayerInputs playerControls;
    private void OnEnable()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        playerMovement = FindObjectOfType<PlayerMovement>().gameObject.GetComponent<PlayerMovement>();
        chatField = GetComponent<InputField>();
        if (playerControls == null)
        {
            playerControls = new PlayerInputs();
            playerControls.ChatInputs.Enable();
        }
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.ChatInputs.Disable(); // Disable only the Chat input
    }
    public void Start()
    {
        
    }

    public void Update()
    {
        CheckInutFieldFocus();
        CheckChatInput();
    }
    public void CheckInutFieldFocus()
    {
        if (chatField.isFocused)
        {
            playerMovement.movementSpeed = 0;
            playerMovement.rotationSpeed = 0;
            cameraManager.cameraFollowSpeed = 0;

        }
        else if (!chatField.isFocused)
        {
            playerMovement.movementSpeed = 12;
            playerMovement.rotationSpeed = 10;
            cameraManager.cameraFollowSpeed = 0.2f;
        }
    }
    public void CheckChatInput()
    {
        float TabInputValue = playerControls.ChatInputs.Chat.ReadValue<float>();
        float EnterInputValue = playerControls.ChatInputs.SendChat.ReadValue<float>();
        if (Mathf.Abs(TabInputValue) > 0.1f)
        {
            Debug.Log("tab");
            chatField.Select();
        }
        if (Mathf.Abs(EnterInputValue) > 0.1f && !string.IsNullOrEmpty(chatField.text))
        {
            Debug.Log("entertab");
            chatButton.onClick.Invoke();
        }
    }
    

}
