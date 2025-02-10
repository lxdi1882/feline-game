using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Import the TextMeshPro namespace

public class TriggerText : MonoBehaviour
{
    public TextMeshProUGUI textToToggle; // Reference to the TextMeshPro component
    public GameObject gameobject; // Reference to another GameObject

    private void Start()
    {
        if (textToToggle != null)
        {
            textToToggle.gameObject.SetActive(false); // Ensure the text is initially disabled
        }

        if (gameobject != null)
        {
            gameobject.SetActive(false); // Ensure the other GameObject is initially disabled
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player triggering it
        {
            if (textToToggle != null)
            {
                textToToggle.gameObject.SetActive(true); // Enable the text if assigned
            }

            if (gameobject != null)
            {
                gameobject.SetActive(true); // Enable the other GameObject if assigned
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player triggering it
        {
            if (textToToggle != null)
            {
                textToToggle.gameObject.SetActive(false); // Disable the text if assigned
            }

            if (gameobject != null)
            {
                gameobject.SetActive(false); // Disable the other GameObject if assigned
            }
        }
    }
}
