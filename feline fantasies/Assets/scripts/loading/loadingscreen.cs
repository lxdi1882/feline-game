using UnityEngine;
using UnityEngine.UI;

public class PanelFadeOut : MonoBehaviour
{
    [Header("Fade Settings")]
    [Tooltip("How long it takes for the panel to fade out.")]
    public float fadeDuration = 1.0f;

    [Tooltip("Automatically start fading when the script runs.")]
    public bool startFadeOnAwake = false;

    private CanvasGroup canvasGroup;
    private float fadeTimer;
    private bool isFading;

    private void Awake()
    {
        // Get the CanvasGroup component (required for fading).
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup component missing. Please add one to the GameObject.");
        }

        if (startFadeOnAwake)
        {
            StartFadeOut();
        }
    }

    private void Update()
    {
        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, fadeTimer / fadeDuration);

            if (canvasGroup != null)
            {
                canvasGroup.alpha = alpha;
            }

            if (fadeTimer >= fadeDuration)
            {
                isFading = false;
                fadeTimer = 0f;
                // Optionally disable the GameObject when fading completes.
                gameObject.SetActive(false);
            }
        }
    }

    public void StartFadeOut()
    {
        if (canvasGroup != null)
        {
            isFading = true;
            fadeTimer = 0f;
        }
    }
}