using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    public Transform focusTransform;
    public Transform playerFocus;
    private float defaultPosition;
    public LayerMask collisionLayers;
    private Vector3 cameraVectorPosition;
    private Transform cameraTransform;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public float cameraFollowSpeed = 0.2f;
    public float lookangle, pivotangle;
    private float cameraLookSpeed = 0.2f;
    private float cameraPivotSpeed = 0.2f;
    public float minPivotAng = -35;
    public float maxPivotAng = 35;
    public float cameraCollisionOffset = 0.2f;
    public float minCollisionOffset = 0.2f;

    public Transform cameraPivot;



    private void Awake()
    {
        focusTransform = FindObjectOfType<PlayerManager>().transform;
        inputManager = FindAnyObjectByType<InputManager>();
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
        playerFocus = focusTransform;
    }
    public void HandleAllCameraMovement()
    {
        FollowPlayer();
        RotateCam();
        HandleCameraColliditon();
    }
    private void FollowPlayer()
    {
        Vector3 targetPos = Vector3.SmoothDamp(transform.position, focusTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPos;
    }
    private void RotateCam()
    {
        Vector3 rotation;

        lookangle = lookangle + (inputManager.cameraInputX * cameraLookSpeed);
        pivotangle = pivotangle - (inputManager.cameraInputY * cameraPivotSpeed);
        pivotangle = Mathf.Clamp(pivotangle, minPivotAng, maxPivotAng);


        rotation = Vector3.zero;
        rotation.y = lookangle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotangle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;

    }
    private void HandleCameraColliditon()
    {
        float targetPositon = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position,0.2f,direction,out hit,Mathf.Abs(targetPositon),collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPositon =- (distance - cameraCollisionOffset);
        }
        if(Mathf.Abs(targetPositon)< minCollisionOffset)
        {
            targetPositon = targetPositon - minCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPositon, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
