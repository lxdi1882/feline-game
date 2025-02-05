using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class TriggerText : MonoBehaviour
{
    public TextMeshProUGUI textToToggle; // Reference to the TextMeshPro component

    private void Start()
    {
        if (textToToggle != null)
        {
            textToToggle.gameObject.SetActive(false); // Ensure the text is initially disabled
        }
        else
        {
            Debug.LogError("TextMeshProUGUI reference is missing. Assign it in the inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (textToToggle != null && other.CompareTag("Player")) // Adjust the tag as needed
        {
            textToToggle.gameObject.SetActive(true); // Enable the text
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (textToToggle != null && other.CompareTag("Player")) // Adjust the tag as needed
        {
            textToToggle.gameObject.SetActive(false); // Disable the text
        }
    }
}
