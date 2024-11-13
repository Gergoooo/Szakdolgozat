using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public GameObject victoryScreen;
    public GameObject hud;
    public EnemySpawner enemySpawner;
    public AudioScript backgroundAudio;

    private void Awake()
    {
        backgroundAudio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioScript>();
    }

    private bool hasEnemiesSpawned = false; 

    void LateUpdate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (!hasEnemiesSpawned && enemies.Length > 0)
        {
            hasEnemiesSpawned = true;
        }

        if (hasEnemiesSpawned && enemies.Length == 0 && !victoryScreen.activeSelf)
        {
            TriggerVictory();
            UnlockNewLevel();
        }
    }
    public void TriggerVictory()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
            victoryScreen.SetActive(true);
            if (hud != null)
            {
                hud.SetActive(false); 
            }
            Time.timeScale = 0;

            if (backgroundAudio != null)
            {
                backgroundAudio.SFXSource.Pause();
            }
        }
    }
    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToNextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void UnlockNewLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}