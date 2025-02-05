using UnityEngine;

public class Bush : MonoBehaviour
{
    private string originalTag; // To store the player's original tag
    private SpriteRenderer spriteRenderer; // To change the bush's opacity
    private AudioSource audioSource; // To play sound when entering

    private void Start()
    {
        // Get the SpriteRenderer component of the bush
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Get the AudioSource component (make sure it's attached to the bush object)
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Store the player's original tag and change to "Hidden"
            originalTag = collision.tag;
            collision.tag = "Hidden";

            // Play the audio
            if (audioSource != null)
            {
                audioSource.Play();
            }

            // Change bush opacity to 50%
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 0.5f; // Set alpha to 50%
                spriteRenderer.color = color;
            }

            Debug.Log("Player entered the bush. Tag changed, audio played, and bush opacity changed.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hidden"))
        {
            // Reset the player's tag to the original
            collision.tag = originalTag;

            // Reset bush opacity to 100%
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 1f; // Set alpha back to 100%
                spriteRenderer.color = color;
            }

            Debug.Log("Player exited the bush. Tag reset and bush opacity restored.");
        }
    }
}
