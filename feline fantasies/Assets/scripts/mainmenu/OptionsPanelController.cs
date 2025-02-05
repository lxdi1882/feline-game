using UnityEngine;

public class OptionsPanelController : MonoBehaviour
{
    public Animator animator; // Reference to the Animator
    public GameObject optionsPanel; // Reference to the panel GameObject
    public PLAYERCONTROL2 playerControl; // Assign the player movement script in the Inspector
    private bool isOpen = false; // Initial state of the panel
    private bool isAnimating = false; // Track if an animation is in progress

    public void TogglePanel()
    {
        // Prevent toggling while an animation is in progress
        if (isAnimating)
        {
            Debug.Log("Animation in progress, ignoring toggle.");
            return;
        }

        isAnimating = true; // Mark as animating

        if (!isOpen)
        {
            optionsPanel.SetActive(true); // Enable the panel if opening
            StartCoroutine(EnablePanelAfterAnimation("OpenPanel")); // Handle opening animation
            if (playerControl != null)
            {
                playerControl.enabled = false; // Disable player movement
                playerControl.rb.linearVelocity = Vector2.zero; // Stop movement immediately
            }

        }
        else
        {
            StartCoroutine(DisablePanelAfterAnimation()); // Handle closing animation
            if (playerControl != null)
            {
                playerControl.enabled = true; // Enable player movement
            }
        }

        isOpen = !isOpen; // Toggle the state
        animator.SetBool("isOpen", isOpen); // Update the Animator parameter
        Debug.Log($"Toggled panel. isOpen: {isOpen}");
    }

    private System.Collections.IEnumerator DisablePanelAfterAnimation()
    {
        // Wait for the ClosePanel animation to finish
        float clipLength = GetAnimationClipLength("ClosePanel");
        if (clipLength > 0)
        {
            yield return new WaitForSeconds(clipLength);
        }

        optionsPanel.SetActive(false); // Disable the panel after animation
        isAnimating = false; // Mark as not animating
        Debug.Log("Panel closed.");
    }

    private System.Collections.IEnumerator EnablePanelAfterAnimation(string animationName)
    {
        // Wait for the specified animation to finish
        float clipLength = GetAnimationClipLength(animationName);
        if (clipLength > 0)
        {
            yield return new WaitForSeconds(clipLength);
        }

        isAnimating = false; // Mark as not animating after opening
        Debug.Log("Panel opened.");
    }

    private float GetAnimationClipLength(string clipName)
    {
        // Get the runtime animator controller
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        // Loop through all animation clips to find the one that matches the given name
        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }

        Debug.LogWarning("Animation clip not found: " + clipName);
        return 0f; // Default to 0 if the clip isn't found
    }
}
