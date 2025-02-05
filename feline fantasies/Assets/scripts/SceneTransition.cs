using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public GameObject fadePanel; // Assign FadePanel in the Inspector
    public float fadeDuration = 1.5f; // Duration of the fade
    public float loadingScreenDuration = 5f; // Total duration of the loading screen

    public TextMeshProUGUI gameplayTipsText; // Assign Gameplay Tips Text in the Inspector
    public TextMeshProUGUI loadingText; // Assign Loading Text in the Inspector

    public string[] gameplayTips; // Array of tips to display
    public float tipChangeInterval = 2f; // How often to change the tip

    public GameObject[] uiElements; // Array of UI elements to disable

    private Image fadeImage;

    void Start()
    {
        // This method is now reserved for Unity's lifecycle
        InitializeTransition();
    }

    public void InitializeTransition()
    {
        // Get the Image component from the fade panel
        fadeImage = fadePanel.GetComponent<Image>();

        // Ensure the panel starts fully opaque (black)
        fadeImage.color = new Color(0, 0, 0, 1);

        // Start the loading sequence
        StartCoroutine(LoadingSequence());
        StartCoroutine(CycleGameplayTips());
        StartCoroutine(AnimateLoadingText());
    }

    IEnumerator LoadingSequence()
    {
        // Wait for the loading screen duration
        yield return new WaitForSeconds(loadingScreenDuration);

        // Fade out the panel
        yield return StartCoroutine(FadeOut());

        // Disable the panel once fade-out is complete
        fadePanel.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        // Disable UI elements at the start of the fade
        foreach (var element in uiElements)
        {
            element.SetActive(false); // Deactivate UI elements
        }

        float timer = 0f;
        Color startColor = fadeImage.color; // Initial color (opaque)
        Color endColor = new Color(0, 0, 0, 0); // Fully transparent

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration); // Smoothly interpolate the color
            yield return null; // Wait for the next frame
        }

        // Ensure the color is fully transparent after the loop
        fadeImage.color = endColor;
    }

    IEnumerator CycleGameplayTips()
    {
        // Randomly cycle through gameplay tips
        while (true)
        {
            string randomTip = gameplayTips[Random.Range(0, gameplayTips.Length)];
            gameplayTipsText.text = randomTip;
            yield return new WaitForSeconds(tipChangeInterval);
        }
    }

    IEnumerator AnimateLoadingText()
    {
        // Animate "Loading." -> "Loading.." -> "Loading..."
        string baseText = "Loading";
        int dotCount = 0;

        while (true)
        {
            dotCount = (dotCount % 3) + 1; // Cycle dot count between 1 and 3
            loadingText.text = baseText + new string('.', dotCount);
            yield return new WaitForSeconds(0.5f); // Adjust speed of animation
        }
    }
}
