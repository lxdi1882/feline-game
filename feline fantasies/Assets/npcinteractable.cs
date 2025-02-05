using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    private bool playerInRange = false;  // Track if the player is in range
    public DialogueTrigger dialogueTrigger;  // Reference to the DialogueTrigger script
    public GameObject interactionIcon;  // Reference to the interaction icon (child of the NPC)

    void Start()
    {
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false); // Hide the icon at the start
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interacting with NPC!");
            dialogueTrigger.TriggerDialogue();  // Trigger dialogue
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;  // Player is in range
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(true); // Show the icon
            }
            Debug.Log("Player entered NPC's range. Press E to interact.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;  // Player left range
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(false); // Hide the icon
            }
            Debug.Log("Player left NPC's range.");
        }
    }
}