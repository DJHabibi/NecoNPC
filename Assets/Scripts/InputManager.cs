using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerInputs playerControls;
    [SerializeField] Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX, cameraInputY;
    public float verticalInput;
    public float horizontalInput;
    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerInputs();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

        }
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    public void HandleAllInputs()
    {
        HandleMovementInput();
        //HandleJumpInput();
        //HandleactionInput();
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

    }
}
