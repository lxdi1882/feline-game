using UnityEngine;

public class animatorhandler : MonoBehaviour
{
    public float speed = 1.0f; // Movement speed for the NPC
    public Animator npcAnimator; // Animator component for the NPC

    private Vector2 lastPosition;      // Stores the NPC's previous position
    private Vector2 lastMoveDirection; // Stores the NPC's last movement direction
    private bool facingLeft = true;    // Tracks whether the NPC is facing left

    void Start()
    {
        // Initialize the last position as the current position
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate movement direction based on position changes
        Vector2 currentPosition = transform.position;
        Vector2 movement = currentPosition - lastPosition;

        // Update animation parameters based on movement
        Animate(movement);

        // Flip the NPC if its movement direction changes
        if (movement.x < 0 && !facingLeft || movement.x > 0 && facingLeft)
        {
            Flip();
        }

        // Update the last position for the next frame
        lastPosition = currentPosition;
    }

    void Animate(Vector2 movement)
    {
        // Calculate movement magnitude
        float movementMagnitude = movement.magnitude;

        // Update the animator parameters
        npcAnimator.SetFloat("MoveX", movement.x);
        npcAnimator.SetFloat("MoveY", movement.y);
        npcAnimator.SetFloat("MoveMagnitude", movementMagnitude);

        // Update the last movement direction if the NPC is moving
        if (movementMagnitude > 0)
        {
            lastMoveDirection = movement.normalized;
        }

        npcAnimator.SetFloat("LastMoveX", lastMoveDirection.x);
        npcAnimator.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    void Flip()
    {
        // Flip the NPC by inverting its local scale on the X axis
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }
}
