using UnityEngine;
using UnityEngine.UI; // Import to work with UI elements like Text

public class SquirrelInteraction : MonoBehaviour
{
    public GameObject dialoguePanel; // Panel to show the dialogue
    public Text dialogueText; // Text UI to display dialogue
    public string[] dialogueLines; // Array to hold the dialogue lines

    private int currentLine = 0; // Keeps track of the current line of dialogue

    void OnMouseDown() // Detects when the squirrel is clicked
    {
        if (!dialoguePanel.activeSelf) // If the panel isn't already showing
        {
            dialoguePanel.SetActive(true); // Show the dialogue panel
            ShowNextDialogue(); // Start showing the dialogue
        }
        else // If the panel is showing already
        {
            ShowNextDialogue(); // Show the next line of dialogue
        }
    }

    void ShowNextDialogue() // Shows the next line of dialogue
    {
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine]; // Display current dialogue line
            currentLine++; // Move to the next dialogue line
        }
        else // If all dialogue lines are shown
        {
            dialoguePanel.SetActive(false); // Hide the dialogue panel
            currentLine = 0; // Reset the dialogue to the first line for next interaction
        }
    }
}
