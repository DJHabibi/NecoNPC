using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    InputManager inputmanager;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidBody;
    public Transform groundCheckTransform;
    public LayerMask collisionLayer;
    public float movementSpeed = 7;
    public float rotationSpeed = 10;
    private bool Running;
    Animator animator;
    public bool grounded;

    public void Awake()
    {
        inputmanager = GetComponent<InputManager>();
        playerRigidBody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        animator = GetComponent<Animator>();
    }
    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputmanager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputmanager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        playerRigidBody.velocity = movementVelocity;
       
        if (playerRigidBody.velocity.x != 0 && playerRigidBody.velocity.z != 0)
        {
            Running = true;
            animator.SetBool("Running", Running);
        }
        if (playerRigidBody.velocity.x == 0 && playerRigidBody.velocity.z == 0)
        {
            Running = false;
            animator.SetBool("Running", Running);
        }
       
    }
    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputmanager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputmanager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
           targetDirection = transform.forward;
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

}
