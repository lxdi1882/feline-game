using UnityEngine;

public class NPCFollower : MonoBehaviour
{
    public float followSpeed = 2f;           // Speed at which the NPC follows the player
    public float followDistance = 1.5f;     // Distance to maintain between the NPC and the player
    public float maximumFollowRange = 10f;  // Maximum distance before the NPC stops following the player
    public GameObject interactionPrompt;    // GameObject for the interaction icon prompt

    private Transform player;               // Reference to the player
    private bool isFollowing = false;       // Whether the NPC is following the player
    private bool interactionTriggered = false; // Whether interaction has been triggered

    private Vector3 defaultPosition;        // Default position to return to when out of range
    private bool isReturningToDefault = false; // Whether the NPC is returning to its default position

    // Static counter to track how many NPCs are following the player
    public static int followingCount = 0;

    private void Start()
    {
        // Store the NPC's default position
        defaultPosition = transform.position;

        // Ensure the prompt is initially hidden
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !interactionTriggered) // Show prompt only if not triggered
        {
            player = other.transform;       // Store the player's transform
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true); // Show the interaction prompt
            }
            Debug.Log("Player in range. Press 'E' to make the NPC follow.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false); // Hide the interaction prompt when leaving range
            }
            Debug.Log("Player left range, but NPC won't unfollow until max range is exceeded.");
        }
    }

    private void Update()
    {
        if (player != null && Input.GetKeyDown(KeyCode.E) && !interactionTriggered) // Only allow interaction once
        {
            isFollowing = !isFollowing;    // Toggle follow state
            interactionTriggered = true;  // Mark interaction as triggered

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false); // Hide the prompt permanently
            }

            if (isFollowing)
            {
                followingCount++;  // Increment the count when following
            }
            else
            {
                followingCount--;  // Decrement the count when stopped
            }

            Debug.Log(isFollowing ? "NPC is now following." : "NPC stopped following.");
            Debug.Log("Total NPCs following: " + followingCount);
        }

        if (isFollowing && player != null)
        {
            FollowPlayer();                // Make the NPC follow the player
            CheckFollowRange();            // Check if the player is beyond the max range
        }
        else if (isReturningToDefault)
        {
            ReturnToDefaultPosition();     // Move the NPC back to its default position
        }
    }

    private void FollowPlayer()
    {
        // Calculate the direction to the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Calculate the distance to the player
        float distance = Vector2.Distance(player.position, transform.position);

        // Move the NPC closer if the distance is greater than the follow distance
        if (distance > followDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
        }
    }

    private void CheckFollowRange()
    {
        // Calculate the distance to the player
        float distance = Vector2.Distance(player.position, transform.position);

        // Stop following if the player exceeds the maximum follow range
        if (distance > maximumFollowRange)
        {
            isFollowing = false;
            followingCount--;  // Decrement the count when NPC stops following
            isReturningToDefault = true; // Start returning to default position
            Debug.Log("Player exceeded max follow range. NPC stopped following.");
            Debug.Log("Total NPCs following: " + followingCount);
        }
    }

    private void ReturnToDefaultPosition()
    {
        // Move the NPC back to its default position
        transform.position = Vector2.MoveTowards(transform.position, defaultPosition, followSpeed * Time.deltaTime);

        // Check if the NPC has reached its default position
        if (Vector2.Distance(transform.position, defaultPosition) < 0.1f)
        {
            isReturningToDefault = false; // Stop returning once the default position is reached
            interactionTriggered = false; // Reset interaction flag to allow interaction again
            Debug.Log("NPC has returned to its default position and is ready for interaction.");

            // Show the interaction prompt only if the player is nearby
            if (player != null && Vector2.Distance(transform.position, player.position) <= GetComponent<CircleCollider2D>().radius)
            {
                if (interactionPrompt != null)
                {
                    interactionPrompt.SetActive(true);
                }
            }
        }
    }
}