using UnityEngine;

public class InteractionIconController : MonoBehaviour
{
    public GameObject interactionIcon;  // Reference to the interaction icon (child of the tree)
    private bool playerInRange = false; // Track if the player is in range

    void Start()
    {
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false); // Initially hide the interaction icon
        }
        else
        {
            Debug.LogWarning("Interaction icon is not assigned!");
        }
    }

    void Update()
    {
        // If the player is in range and presses the "E" key, trigger the interaction (for testing purposes)
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interacting with the tree!");
            // You can call the tree interaction method here if needed
        }
    }

    // When the player enters the interaction range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(true); // Show the icon when player is in range
                Debug.Log("Player is in range, interaction icon visible.");
            }
        }
    }

    // When the player leaves the interaction range
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(false); // Hide the icon when player is out of range
                Debug.Log("Player left range, interaction icon hidden.");
            }
        }
    }
}
