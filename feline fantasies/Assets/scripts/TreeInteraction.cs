using System.Collections;
using UnityEngine;

public class TreeInteraction : MonoBehaviour
{
    public GameObject leafPrefab; // The leaf to drop
    public Transform dropPoint;  // Where the leaf will drop
    public float shakeAmount = 0.2f; // How much the tree moves when shaking
    public float shakeDuration = 0.5f; // How long the shake lasts
    public int maxInteractions = 2; // Maximum number of times leaves can drop
    public float interactionCooldown = 1f; // Cooldown time between interactions
    public AudioClip shakeSound; // Sound for tree shaking
    public AudioClip leafDropSound; // Sound for dropping leaves
    public GameObject interactionIcon; // Reference to the interaction icon (child of the tree)

    public GameObject[] objectsToDisable; // Array of game objects to disable when no more interactions are available

    private int interactionCount = 0; // Tracks how many times the tree has been interacted with
    private Vector3 originalPosition; // To reset the tree position after shaking
    private bool canInteract = true; // To prevent spamming interactions
    private AudioSource audioSource; // To play sounds
    private bool playerInRange = false; // To track if the player is in range

    void Start()
    {
        originalPosition = transform.position; // Store the original position
        audioSource = GetComponent<AudioSource>();

        // Add an AudioSource component if it doesn’t exist
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Hide the interaction icon at the start
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false);
            Debug.Log("Interaction icon is initially hidden.");
        }
        else
        {
            Debug.LogWarning("Interaction icon is not assigned!");
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interacting with tree!");
            InteractWithTree();
        }
    }

    public void InteractWithTree()
    {
        // Prevent interaction during cooldown
        if (!canInteract) return;

        Debug.Log("Interaction with tree started.");
        // Start interaction
        StartCoroutine(HandleInteraction());
    }

    private IEnumerator HandleInteraction()
    {
        // Disable interaction during cooldown
        canInteract = false;

        // Play the shake sound
        if (shakeSound != null)
        {
            audioSource.PlayOneShot(shakeSound);
            Debug.Log("Shake sound played.");
        }

        // Shake the tree
        yield return StartCoroutine(ShakeTree());

        // Drop a leaf only if the interaction count is less than the max
        if (interactionCount < maxInteractions)
        {
            interactionCount++;
            DropLeaf();
        }

        // If no more interactions are possible, disable objects
        if (interactionCount >= maxInteractions)
        {
            DisableObjects();
        }

        // Wait for the cooldown duration
        yield return new WaitForSeconds(interactionCooldown);

        // Re-enable interaction
        canInteract = true;
    }

    private IEnumerator ShakeTree()
    {
        float elapsedTime = 0f;

        // Shake the tree
        while (elapsedTime < shakeDuration)
        {
            transform.position = originalPosition + (Vector3.right * Mathf.Sin(elapsedTime * 20f) * shakeAmount);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset position after shaking
        transform.position = originalPosition;
    }

    private void DropLeaf()
    {
        if (leafPrefab != null && dropPoint != null)
        {
            Instantiate(leafPrefab, dropPoint.position, Quaternion.identity);

            // Play the leaf drop sound
            if (leafDropSound != null)
            {
                audioSource.PlayOneShot(leafDropSound);
                Debug.Log("Leaf drop sound played.");
            }
        }
        else
        {
            Debug.LogWarning("LeafPrefab or DropPoint is missing.");
        }
    }

    private void DisableObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log("Disabled: " + obj.name);
            }
            else
            {
                Debug.LogWarning("Object in the disable array is null.");
            }
        }
    }

    // When the player enters the tree's interaction range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered tree's range.");
            playerInRange = true; // Player is in range
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(true); // Show the interaction icon
                Debug.Log("Interaction icon is now visible.");
            }
            else
            {
                Debug.LogWarning("Interaction icon is not assigned.");
            }
        }
        else
        {
            Debug.Log("Something else entered the tree's range.");
        }
    }

    // When the player exits the tree's interaction range
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left tree's range.");
            playerInRange = false; // Player is out of range
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(false); // Hide the interaction icon
                Debug.Log("Interaction icon is now hidden.");
            }
        }
        else
        {
            Debug.Log("Something else left the tree's range.");
        }
    }
}
