using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TriggerScript : MonoBehaviour
{
    public PLAYERCONTROL2 playerControlScript;  // Reference to the player movement script

    public GameObject[] objectsToEnable;  // Array of game objects to enable after the fade
    public AudioSource audioSource;  // Reference to the AudioSource component to play sound
    public float fadeInTime = 2f;  // Time to fade in the panel
    public FadingInAndOut fadePanel;
    public GameObject deathPanel;

    private bool hasTriggered = false;
    private bool hasAudioPlayed = false;  // Add a flag to prevent multiple sound plays

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger has either the "Player" or "Hidden" tag
        if (!hasTriggered && (other.CompareTag("Player") || other.CompareTag("Hidden")))
        {
            Debug.Log("Object entered the trigger!");  // Debug log to confirm detection
            hasTriggered = true;

            // Disable player movement (if required)
            // playerControlScript.enabled = false; // Uncomment if needed to disable movement

            // Enable the fade panel (if it's disabled in the inspector)
            // If fadePanel is set up, you can activate it here if necessary.

            // Start fading in the panel
            StartCoroutine(TriggerFadePanelAfterDialogue());

            // Play sound only once when the object enters the trigger
            if (!hasAudioPlayed && audioSource != null)
            {
                audioSource.Play();
                Debug.Log("Sound played! from trigger script");  // Debug log to confirm sound is played
                hasAudioPlayed = true;  // Set the flag to prevent future audio plays
            }
        }
    }

    private IEnumerator TriggerFadePanelAfterDialogue()
    {
        // Wait a bit for the dialogue to fully close (adjust time as needed)
        yield return new WaitForSeconds(0f);

        // Trigger fade panel after quest completion and dialogue end
        deathPanel.gameObject.SetActive(true);
        fadePanel.StartCoroutine(fadePanel.FadeInOut());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Hidden"))
        {
            hasTriggered = false;
            hasAudioPlayed = false;
        }
    }
}
