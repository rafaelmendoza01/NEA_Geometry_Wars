using UnityEngine;
using UnityEngine.SceneManagement; //used to change scenes in Unity

public class MainMenu : MonoBehaviour
{
    //To load up the game and set up necessary stats/settings.
    public void PlayGame()
    {
        PlayerMovement.KillsForLevel = 0;
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
