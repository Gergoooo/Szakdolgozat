using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject GameOver;
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject HUD;

    new AudioScript audio;

    private void Awake()
    {
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioScript>();
    }

    public void ShowGameOverScreen()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        if (HUD != null)
        {
            HUD.SetActive(false);
        }

        if (GameOver != null)
        {
            GameOver.SetActive(true);
            Time.timeScale = 0f; 
            DisableInputs();
        }
    }

    private void DisableInputs()
    {
        GameIsPaused = true;
    }

    private void EnableInputs()
    {
        GameIsPaused = false;
    }

    public void Home()
    {
        Time.timeScale = 1f;
        EnableInputs();
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        EnableInputs();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
