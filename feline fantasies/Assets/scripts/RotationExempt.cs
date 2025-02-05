using UnityEngine;

public class RotationExempt : MonoBehaviour
{
    private Quaternion initialWorldRotation;
    private Animator animator;    // Reference to the Animator component
    private Vector2 lastPosition; // Track the last position of the NPC

    void Start()
    {
        // Save the initial world rotation of the object
        initialWorldRotation = transform.rotation;

        // Get the Animator component
        animator = GetComponent<Animator>();
        lastPosition = transform.position; // Initialize the last position
    }

    void LateUpdate()
    {
        // Lock the object's rotation
        transform.rotation = initialWorldRotation;

        // Check movement and update the animation
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        // Calculate the current movement direction
        Vector2 currentPosition = transform.position;
        Vector2 direction = (currentPosition - lastPosition).normalized;

        // Check if the object is moving
        bool isMoving = (Vector2)transform.position != lastPosition;

        // Update the Animator parameters
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            // Update directional parameters for the animator
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
        }

        // Store the current position for the next frame
        lastPosition = currentPosition;
    }
}
