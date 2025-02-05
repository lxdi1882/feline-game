using UnityEngine;
using UnityEngine.UI; // Import the UI namespace for the Image component
using System.Collections;

public class FadingInAndOut : MonoBehaviour
{
    public float fadeDuration = 1f; // Time to fade in and fade out
    public float stayDuration = 2f; // Time to stay at full opacity
    public GameObject objectToTeleport; // The game object to teleport
    public GameObject targetLocation; // The empty GameObject to teleport to
    public GameObject[] objectsToEnable; // Array of GameObjects to enable when opacity is 100%
    public GameObject[] objectsToDisable; // Array of GameObjects to disable
    public GameObject Text;
    private Image imageComponent; // Used for UI Image components


    void Awake()
    {
        // Try to get the Image component (for UI objects like Panels)
        imageComponent = GetComponent<Image>();

        // Log if the Image component is found
        if (imageComponent != null)
        {
            Debug.Log("Image component found. Ready to fade UI object.");
        }
        else
        {
            Debug.LogError("No Image component found. Please attach the Image component to the GameObject.");
        }

        // Start the fade process
        StartCoroutine(FadeInOut());
    }

    public IEnumerator FadeInOut()
    {
        // Fade in
        yield return StartCoroutine(FadeTo(1f, fadeDuration));

        // Enable the specified GameObjects when opacity reaches 100%
        EnableObjects();

        // Teleport the target object after the fade-in is complete (fully visible)
        TeleportObject();

        // Stay at full opacity for a set duration
        yield return new WaitForSeconds(stayDuration);

        // Disable the specified GameObjects when opacity reaches 100% and time is over
        DisableObjects();

        // Fade out
        yield return StartCoroutine(FadeTo(0f, fadeDuration));

        // Log when the fade-out is done
        Debug.Log("Fade-out complete.");

        // Disable the GameObject after it fades out
        gameObject.SetActive(false); // This will disable the GameObject after fade-out
    }

    // Fade based on the Image component
    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        if (imageComponent != null) // UI Object (Image)
        {
            float startAlpha = imageComponent.color.a;
            float time = 0;

            while (time < duration)
            {
                Color newColor = imageComponent.color;
                newColor.a = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                imageComponent.color = newColor;

                time += Time.deltaTime;
                yield return null;
            }

            // Ensure the final alpha is exactly the target
            Color finalColor = imageComponent.color;
            finalColor.a = targetAlpha;
            imageComponent.color = finalColor;
        }
    }

    // Teleport the target object to the target location
    void TeleportObject()
    {
        if (objectToTeleport != null && targetLocation != null)
        {
            objectToTeleport.transform.position = targetLocation.transform.position; // Teleport to the target location
            Debug.Log("Object teleported to: " + targetLocation.name);
        }
        else
        {
            if (objectToTeleport == null)
                Debug.LogWarning("Object to teleport is not assigned.");
            if (targetLocation == null)
                Debug.LogWarning("Target location is not assigned.");
        }
    }

    // Enable all specified GameObjects
    void EnableObjects()
    {
        if (objectsToEnable != null && objectsToEnable.Length > 0)
        {
            foreach (GameObject obj in objectsToEnable)
            {
                if (obj != null)
                {
                    obj.SetActive(true); // Enable each GameObject when opacity reaches 100%
                    Debug.Log("Object enabled: " + obj.name);
                }
                else
                {
                    Debug.LogWarning("One of the objects to enable is null.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No objects to enable or array is empty.");
        }
    }

    // Disable all specified GameObjects
    void DisableObjects()
    {
        if (objectsToDisable != null && objectsToDisable.Length > 0)
        {
            foreach (GameObject obj in objectsToDisable)
            {
                if (obj != null)
                {
                    obj.SetActive(false); // Disable each GameObject
                    Debug.Log("Object disabled: " + obj.name);
                }
                else
                {
                    Debug.LogWarning("One of the objects to disable is null.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No objects to disable or array is empty.");
        }
    }
}
