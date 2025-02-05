using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    public string sceneToLoad = "SampleScene";  // Name of the scene to load when pressing E
    private bool isPlayerInRange = false;

    // Reference to the visual cue text
    public GameObject interactText;

    void Update()
    {
        // If the player is close and presses 'E'
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Load the new scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // When the player enters the door's trigger area
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactText != null) interactText.SetActive(true);  // Show visual cue
        }
    }

    // When the player leaves the door's trigger area
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactText != null) interactText.SetActive(false);  // Hide visual cue
        }
    }
}