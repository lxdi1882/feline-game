using UnityEngine;
using System.Collections;

public class PredatorDetection : MonoBehaviour
{
    public float detectionRange = 5f;  // How far the predator can detect the player
    public float detectionDelay = 0.5f; // Time delay after full opacity before detecting player
    public float fadeDurationMultiplier = 2f; // Multiplier for fade duration based on detection delay

    private Transform player;  // Reference to the player (drag your player here)
    private PredatorMovement predatorMovement;  // Reference to the predator movement script
    private SpriteRenderer detectionConeRenderer;  // Reference to the detection cone's SpriteRenderer
    private bool isPlayerDetected = false;
    private Coroutine fadeCoroutine; // Store the fade coroutine so it can be stopped

    void Start()
    {
        // Optionally, find the player by tag (ensure player object has "Player" tag assigned)
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Get the PredatorMovement script from the predator
        predatorMovement = GetComponentInParent<PredatorMovement>();
        if (predatorMovement == null)
        {
            Debug.LogError("PredatorMovement script not found on parent object!");
        }

        // Get the SpriteRenderer of the detection cone to modify its opacity
        detectionConeRenderer = GetComponent<SpriteRenderer>();
        if (detectionConeRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the detection cone!");
        }
        else
        {
            // Set the initial opacity to 50% when the detection cone starts
            Color initialColor = detectionConeRenderer.color;
            initialColor.a = 0.5f;
            detectionConeRenderer.color = initialColor;
        }
    }

    void Update()
    {
        // Check if the player is within detection range, but only if detection has started
        if (isPlayerDetected && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            // The player is detected, and the predator starts chasing
            predatorMovement.StartChasing();
        }
    }

    // When the player enters the detection trigger area
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Turn the detection cone red immediately when the player enters
            detectionConeRenderer.color = Color.red;

            // Start the fade-in process
            if (fadeCoroutine == null)
            {
                fadeCoroutine = StartCoroutine(FadeInDetectionCone());
            }
            Debug.Log("Player entered detection area.");
        }
    }

    // Coroutine to gradually fade in the detection cone's opacity
    private IEnumerator FadeInDetectionCone()
    {
        float elapsedTime = 0f;
        Color currentColor = detectionConeRenderer.color;

        float fadeDuration = detectionDelay * fadeDurationMultiplier;

        // Gradually increase the alpha value (opacity) from 0.5 to 1 over the fade duration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float lerpedAlpha = Mathf.Lerp(0.5f, 1f, elapsedTime / fadeDuration);
            currentColor.a = lerpedAlpha;
            detectionConeRenderer.color = currentColor;

            yield return null;  // Wait for the next frame
        }

        // After the fade-in is complete, start the detection process
        yield return new WaitForSeconds(detectionDelay);

        // Once the fade is complete, trigger the detection
        isPlayerDetected = true;

        Debug.Log("Player fully detected!");
    }

    // When the player exits the detection range
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop the detection and reset the state if the player exits the trigger area
            isPlayerDetected = false;
            predatorMovement.StopChasing();  // Stop chasing and resume patrolling

            // Stop and reset fading if the player leaves the trigger area
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);  // Stop the fade-in coroutine if it's still running
                fadeCoroutine = null;  // Reset the coroutine reference
            }

            ResetDetectionCone();  // Reset the detection cone's opacity to 50%
            Debug.Log("Player left detection range.");
        }
    }

    // Reset the detection cone to its initial state
    private void ResetDetectionCone()
    {
        Color resetColor = detectionConeRenderer.color;
        resetColor.a = 0.5f;  // Reset the opacity to 50%
        resetColor.r = 1f;  // Reset the red component to 100% for the default color
        resetColor.g = 1f;  // Reset the green component to 100% for the default color
        resetColor.b = 1f;  // Reset the blue component to 100% for the default color
        detectionConeRenderer.color = resetColor;
    }
}
