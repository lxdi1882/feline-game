using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints; // The points where the NPC will move
    public float speed = 2f;      // How fast the NPC moves
    public float stopThreshold = 0.05f; // Threshold for stopping near a waypoint

    private int currentWaypointIndex = 0;
    private Animator animator;    // Reference to the Animator component
    private Vector2 lastPosition; // Track the last position of the NPC

    void Start()
    {
        // Get the Animator component attached to the NPC
        animator = GetComponent<Animator>();
        lastPosition = transform.position; // Initialize the last position at the start
    }

    void Update()
    {
        MoveToNextWaypoint();
    }

    // Move the NPC to the next waypoint in the loop
    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return; // No waypoints assigned

        // Get the direction to the current waypoint
        Vector2 direction = (waypoints[currentWaypointIndex].position - transform.position);

        // Check if the NPC is close enough to the waypoint
        if (direction.magnitude < stopThreshold)
        {
            // NPC reached the waypoint, stop moving and update to idle animation
            animator.SetBool("IsMoving", false); // Set IsMoving to false
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop to the next waypoint
            return;
        }

        // Normalize the direction for movement
        direction.Normalize();

        // Move the NPC towards the current waypoint
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        // Update the animation based on movement direction
        UpdateAnimation(direction);
    }

    // Update the animation based on the movement direction
    void UpdateAnimation(Vector2 direction)
    {
        // Check if the position has changed (i.e., the NPC is moving)
        bool isMoving = (Vector2)transform.position != lastPosition;

        // Update IsMoving based on whether the NPC's position has changed
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            // Update direction parameters (Up, Down, Left, Right)
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
        }

        // Store the current position for the next frame's comparison
        lastPosition = transform.position;
    }
}
