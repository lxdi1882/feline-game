using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign the PauseMenu panel in the Inspector
    public GameObject pauseButton; // Assign the PauseButton in the Inspector

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // You can use any key for pause toggle
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f; // Freeze the game
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f; // Unfreeze the game
        isPaused = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Unfreeze the game if quitting
        SceneManager.LoadScene("menu"); // Load main menu or quit application
    }
}