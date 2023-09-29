using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public float gravityStrength = 9.81f; // Strength of gravity force
    public LayerMask groundLayer; // Layer mask for detecting ground
    public float groundRayLength = 0.1f; // Length of the ground detection ray

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable the built-in Unity gravity
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        // Check if the object is grounded
        bool isGrounded = IsGrounded();

        // If not grounded, apply gravity
        if (!isGrounded)
        {
            Vector3 gravityVector = Vector3.down * gravityStrength;
            rb.AddForce(gravityVector, ForceMode.Acceleration);
        }
    }

    private bool IsGrounded()
    {
        // Create a ray from the center of the object downward to check for ground collision
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        // Check if the ray hits anything on the ground layer
        if (Physics.Raycast(ray, out hit, groundRayLength, groundLayer))
        {
            return true; // The object is grounded
        }

        return false; // The object is not grounded
    }
}
