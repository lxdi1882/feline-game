using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.UI; // Import UI for the icon

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // Reference to the dialogue panel GameObject
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image npcIcon;  // Reference to the Image component where the icon will be displayed
    private Queue<string> sentences;
    public Animator animator;

    public float typingSpeed = 0.05f; // Adjustable typing speed

    void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false); // Disable the panel at the start
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true); // Enable the panel
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        // If the dialogue has an icon, set it
        if (dialogue.icon != null)
        {
            npcIcon.sprite = dialogue.icon;  // Set the icon sprite
            npcIcon.enabled = true;  // Enable the icon
        }
        else
        {
            npcIcon.enabled = false;  // Hide the icon if no icon is set
        }

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // Use the adjustable speed
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");
        animator.SetBool("IsOpen", false);
        npcIcon.enabled = false;  // Hide the icon when the dialogue ends
        StartCoroutine(DisablePanelAfterAnimation());
    }

    IEnumerator DisablePanelAfterAnimation()
    {
        // Wait for the animation to finish before disabling the panel
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        dialoguePanel.SetActive(false); // Disable the panel after the animation ends
    }
}
