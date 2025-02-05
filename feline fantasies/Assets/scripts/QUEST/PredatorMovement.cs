using UnityEngine;

public enum PredatorState
{
    Patrolling,
    Chasing
}

public class PredatorMovement : MonoBehaviour
{
    public Transform[] waypoints; // The points where the predator will move
    public float roamingSpeed = 2f;      // How fast the predator moves while patrolling
    public float chasingSpeed = 4f;      // How fast the predator moves when chasing the player
    public float rotationSpeed = 5f;     // Speed at which the predator rotates
    public float waypointThreshold = 0.1f; // Distance to determine if the predator reached a waypoint

    private int currentWaypointIndex = 0;
    private Transform player;  // Reference to the player
    private PredatorState currentState = PredatorState.Patrolling;  // Start in Patrolling state

    public GameObject chaseEffect; // The GameObject to be enabled during chase

    void Start()
    {
        // Optionally, find the player by tag (ensure player object has "Player" tag assigned)
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }

        // Ensure the chase effect is initially disabled
        if (chaseEffect != null)
        {
            chaseEffect.SetActive(false);
        }
    }

    void Update()
    {
        // Update movement depending on state
        if (currentState == PredatorState.Patrolling)
        {
            MoveToNextWaypoint(roamingSpeed);  // Use roaming speed while patrolling
        }
        else if (currentState == PredatorState.Chasing)
        {
            ChasePlayer(chasingSpeed);  // Use chasing speed while chasing the player
        }
    }

    // Move the predator to the next waypoint in the loop
    void MoveToNextWaypoint(float speed)
    {
        if (waypoints.Length == 0) return; // No waypoints assigned

        // Get the direction to the current waypoint
        Vector2 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;

        // Smoothly rotate the predator towards the target waypoint
        RotateTowards(direction);

        // Move towards the current waypoint
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        // Check if the predator has reached the waypoint
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < waypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop to the next waypoint
        }
    }

    // Chase the player
    void ChasePlayer(float speed)
    {
        if (player == null) return;

        // Get the direction to the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Smoothly rotate the predator towards the player
        RotateTowards(direction);

        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    // Rotate the predator smoothly towards the target direction
    void RotateTowards(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Switch to chasing state
    public void StartChasing()
    {
        currentState = PredatorState.Chasing;
        if (chaseEffect != null)
        {
            chaseEffect.SetActive(true);  // Enable the chase effect when the predator starts chasing
        }
    }

    // Switch to patrolling state
    public void StopChasing()
    {
        currentState = PredatorState.Patrolling;
        if (chaseEffect != null)
        {
            chaseEffect.SetActive(false);  // Disable the chase effect when the predator stops chasing
        }
    }
}
