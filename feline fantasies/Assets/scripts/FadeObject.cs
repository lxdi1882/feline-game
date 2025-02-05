using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    public float fadeDuration = 1f; // Duration of the fade in/out
    public float fadeDelay = 0f;   // Delay between fades
    public bool loop = true;       // Should the fade loop continuously?

    private Renderer objectRenderer;
    private Color objectColor;
    private float fadeTimer = 0f;
    private bool fadingIn = true;

    void Start()
    {
        // Get the Renderer component
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError("Renderer not found on the GameObject. Please attach a Renderer component.");
            return;
        }

        // Get the object's initial color
        objectColor = objectRenderer.material.color;

        // Ensure the material supports transparency
        objectColor.a = 0f; // Start fully transparent
        objectRenderer.material.color = objectColor;
    }

    void Update()
    {
        if (objectRenderer == null) return;

        fadeTimer += Time.deltaTime;

        if (fadingIn)
        {
            objectColor.a = Mathf.Clamp01(fadeTimer / fadeDuration);
            objectRenderer.material.color = objectColor;

            if (fadeTimer >= fadeDuration)
            {
                fadingIn = false;
                fadeTimer = 0f;
                if (!loop) enabled = false;
            }
        }
        else
        {
            objectColor.a = Mathf.Clamp01(1f - (fadeTimer / fadeDuration));
            objectRenderer.material.color = objectColor;

            if (fadeTimer >= fadeDuration)
            {
                fadingIn = true;
                fadeTimer = -fadeDelay; // Add delay before the next fade
            }
        }
    }
}
