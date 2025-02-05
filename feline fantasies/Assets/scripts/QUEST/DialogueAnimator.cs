using UnityEngine;

public class DialogueAnimator : MonoBehaviour
{
    public Animator dialogueAnimator; // Reference to the Animator that controls the dialogue panel's animation
    public string isOpenParameter = "IsOpen"; // The parameter in the Animator that triggers the open/close animation

    private void Start()
    {
        if (dialogueAnimator == null)
        {
            Debug.LogError("Dialogue Animator is not assigned in the Inspector!");
        }
    }

    // Method to open the dialogue panel with animation
    public void OpenDialogue()
    {
        SetIsOpen(true);
    }

    // Method to close the dialogue panel with animation
    public void CloseDialogue()
    {
        SetIsOpen(false);
    }

    // Helper method to set the "IsOpen" parameter in the Animator
    private void SetIsOpen(bool value)
    {
        if (dialogueAnimator != null)
        {
            dialogueAnimator.SetBool(isOpenParameter, value);
        }
    }
}
