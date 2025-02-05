using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;  // The dialogue to be triggered

    public void TriggerDialogue()
    {
        // Find the DialogueManager in the scene and start the dialogue
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogWarning("DialogueManager not found in the scene.");
        }
    }
}
