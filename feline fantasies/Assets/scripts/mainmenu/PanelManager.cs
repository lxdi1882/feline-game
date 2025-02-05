using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject currentPanel; // Reference to the currently active panel

    // Call this function when a button is clicked
    public void OnButtonClicked(GameObject newPanel)
    {
        // If there's an active panel, hide it
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }

        // Show the new panel
        newPanel.SetActive(true);

        // Update the reference to the new active panel
        currentPanel = newPanel;
    }
}
