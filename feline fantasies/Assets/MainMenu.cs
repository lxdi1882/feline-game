using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("HOUSE");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT!!! BICH");
    }
}
