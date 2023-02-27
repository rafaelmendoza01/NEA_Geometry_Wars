using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    //source: https://youtu.be/JivuXdrIHK0
    public static bool GameIsPaused = false;
    [SerializeField]
    private GameObject pauseMenuUI;
    [SerializeField]
    private GameObject scoreMenuUI;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused && pauseMenuUI.active)
            {
                Resume();
            }
            else if(GameIsPaused && scoreMenuUI.active && StatsUpdate.num_lives > 0)
            {
                scoreMenuUI.SetActive(false);
                pauseMenuUI.SetActive(true);
            }
            else if(scoreMenuUI.active == true && StatsUpdate.num_lives == 0)
            {
                SceneManager.LoadScene("Menu");
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Menu");
    }

    public void BringUpLeaderBoard()
    {
        pauseMenuUI.SetActive(false);
        scoreMenuUI.SetActive(true);
    }
}
