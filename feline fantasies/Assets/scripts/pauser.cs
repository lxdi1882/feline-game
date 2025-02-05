using UnityEngine;

public class pauser : MonoBehaviour
{
    public GameObject panel; // Assign the panel in the Inspector
    public PLAYERCONTROL2 playerControl; // Assign the player movement script in the Inspector

    void OnEnable()
    {
        if (playerControl != null)
        {
            playerControl.enabled = false; // Disable player movement
        }
    }

    void OnDisable()
    {
        if (playerControl != null)
        {
            playerControl.enabled = true; // Enable player movement
        }
    }

    public void TogglePanel()
    {
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);
    }
}
