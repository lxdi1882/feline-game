using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove_Ref : MonoBehaviour
{
    public int sceneBuildIndex;
    public GameObject interactionPrompt; // Reference to the GameObject that will pop up (UI element, etc.)
    public GameObject panelToOpen; // Reference to the panel to open when the player presses "E"

    private bool isPlayerInRange = false; // To check if the player is in range

    private void Start()
    {
        // Ensure the interaction prompt and panel are initially hidden
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        if (panelToOpen != null)
        {
            panelToOpen.SetActive(false); // Hide the panel initially
        }
    }

    private void Update()
    {
        // Check for player input if they're in range and they press "E"
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Toggle the panel's visibility when "E" is pressed
            if (panelToOpen != null)
            {
                panelToOpen.SetActive(!panelToOpen.activeSelf); // Toggle the panel's visibility
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // Show the interaction prompt (GameObject)
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true); // Make the GameObject visible
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When the player leaves the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            // Hide the interaction prompt (GameObject)
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false); // Make the GameObject invisible
            }
        }
    }
}
