using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Add a public SceneAsset variable to allow scene selection in the Inspector
    public string sceneName;

    // Method to load the selected scene by its name
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
