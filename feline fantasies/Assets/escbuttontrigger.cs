using UnityEngine;
using UnityEngine.UI;

public class EscapeToggle : MonoBehaviour
{
    public Button pauseButton; // Assign the pause button in the Inspector
    public Button closeButton; // Assign the close button in the Inspector
    private bool isPaused = false; // Track toggle state
    private bool canToggle = true; // Cooldown flag
    private float cooldownTime = 2f; // Cooldown duration

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canToggle)
        {
            if (isPaused)
            {
                closeButton?.onClick.Invoke(); // Trigger close button
            }
            else
            {
                pauseButton?.onClick.Invoke(); // Trigger pause button
            }

            isPaused = !isPaused; // Toggle state
            StartCoroutine(Cooldown()); // Start cooldown
        }
    }

    private System.Collections.IEnumerator Cooldown()
    {
        canToggle = false;
        yield return new WaitForSeconds(cooldownTime);
        canToggle = true;
    }
}
