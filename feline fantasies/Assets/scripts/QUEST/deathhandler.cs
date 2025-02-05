using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathHandler : MonoBehaviour
{
    public GameObject deathPanel; // Assign the panel GameObject
    public Button respawnButton; // Assign the button on the panel
    public Transform respawnLocation; // Assign the GameObject's Transform to teleport to
    public float fadeDuration = 1f; // Duration for fading in/out
    public SceneTransition sceneTransition; // Reference to SceneTransition script

    private CanvasGroup panelCanvasGroup; // Internal CanvasGroup for fade effect
    private bool isFading = false;

    private void Start()
    {
        // Add a CanvasGroup if it doesn't exist
        if (deathPanel != null)
        {
            if (!deathPanel.TryGetComponent(out panelCanvasGroup))
            {
                panelCanvasGroup = deathPanel.AddComponent<CanvasGroup>();
            }

            // Ensure the panel starts hidden
            deathPanel.SetActive(false);
            panelCanvasGroup.alpha = 0f;
        }

        // Add the button click listener
        if (respawnButton != null)
        {
            respawnButton.onClick.AddListener(OnRespawnButtonClicked);
            respawnButton.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFading && deathPanel != null)
        {
            StartCoroutine(FadeInPanel());
        }
    }

    private IEnumerator FadeInPanel()
    {
        isFading = true;
        deathPanel.SetActive(true);
        float elapsed = 0f;

        // Fade in the panel
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        if (respawnButton != null)
        {
            respawnButton.gameObject.SetActive(true);
        }

        isFading = false;
    }

    private void OnRespawnButtonClicked()
    {
        // Teleport the player
        if (respawnLocation != null)
        {
            transform.position = respawnLocation.position;
        }

        // Immediately disable the button
        if (respawnButton != null)
        {
            respawnButton.interactable = false;
        }

        // Find the SceneTransition component and start the transition
        SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
        if (sceneTransition != null)
        {
            sceneTransition.InitializeTransition();
        }

        // Start fading out after a delay
        StartCoroutine(FadeOutPanel());
    }

    private IEnumerator FadeOutPanel()
    {
        yield return new WaitForSeconds(1f);

        isFading = true;
        float elapsed = 0f;

        // Disable the button
        if (respawnButton != null)
        {
            respawnButton.gameObject.SetActive(false);
        }

        // Fade out the panel
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            panelCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        deathPanel.SetActive(false);
        isFading = false;
    }
}
