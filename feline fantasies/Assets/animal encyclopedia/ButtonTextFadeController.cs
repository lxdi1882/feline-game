using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ButtonTextFadeController : MonoBehaviour
{
    public Button targetButton; // Reference to the button you want to control
    public TextMeshProUGUI buttonText; // Reference to the button's TextMeshPro component
    public float fadeDuration = 1f; // Duration for the fade effect

    private void Start()
    {
        // Initialize the button and text references if not assigned in the Inspector
        if (targetButton == null)
            targetButton = GetComponent<Button>();
        if (buttonText == null)
            buttonText = targetButton.GetComponentInChildren<TextMeshProUGUI>();

        // Initially, set the text alpha to 0 (invisible)
        Color textColor = buttonText.color;
        textColor.a = 0f;
        buttonText.color = textColor;
    }

    // Public method to start the button text fade effect
    public void StartButtonTextFade()
    {
        StartCoroutine(FadeButtonText());
    }

    private IEnumerator FadeButtonText()
    {
        // Fade in (from alpha 0 to 1)
        yield return FadeText(0f, 1f);

        // Wait for a moment (you can adjust this time as needed)
        yield return new WaitForSeconds(1f);

        // Fade out (from alpha 1 to 0)
        yield return FadeText(1f, 0f);
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color startColor = buttonText.color;
        startColor.a = startAlpha;

        Color endColor = startColor;
        endColor.a = endAlpha;

        while (elapsedTime < fadeDuration)
        {
            buttonText.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        buttonText.color = endColor; // Ensure the final color is applied
    }
}
