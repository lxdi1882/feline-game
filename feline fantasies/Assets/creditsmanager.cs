using System.Collections;
using UnityEngine;
using TMPro;

public class CreditsFadeIn : MonoBehaviour
{
    public TextMeshProUGUI creditsText;
    public float fadeDuration = 2f;
    public float delayBeforeStart = 1f;
    public float delayBetweenEntries = 1f;

    [TextArea(10, 30)]
    public string[] creditsEntries;

    void Start()
    {
        creditsText.text = "";
        creditsText.alpha = 0f;
        StartCoroutine(PlayCreditsSequentially());
    }

    IEnumerator PlayCreditsSequentially()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        foreach (string entry in creditsEntries)
        {
            yield return StartCoroutine(FadeInOutEntry(entry));
            yield return new WaitForSeconds(delayBetweenEntries);
        }
    }

    IEnumerator FadeInOutEntry(string entry)
    {
        float elapsedTime = 0f;
        creditsText.text = entry;

        while (elapsedTime < fadeDuration)
        {
            creditsText.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(fadeDuration);

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            creditsText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
