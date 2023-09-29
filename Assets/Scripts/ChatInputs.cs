using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ChatInputs : MonoBehaviour
{
    private InputField chatField;
    public Button chatButton;
    public PlayerMovement playerMovement;
    public CameraManager cameraManager;
    private void OnEnable()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        playerMovement = FindObjectOfType<PlayerMovement>().gameObject.GetComponent<PlayerMovement>();
        chatField = GetComponent<InputField>();
    }

    public void Update()
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

}
