using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // This is needed for IEnumerator

public class PublisherScreen : MonoBehaviour
{
    public float displayTime = 3f; // Time to display the logo
    public string nextSceneName = "MainMenu"; // Name of the next scene

    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        // Wait for the display time
        yield return new WaitForSeconds(displayTime);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
