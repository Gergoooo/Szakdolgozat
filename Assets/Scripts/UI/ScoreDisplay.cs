using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  
    public DestroyedEnemyCount destroyedEnemyCountScript;

    private void Start()
    {
        // Optionally, check if the references are set
        if (destroyedEnemyCountScript == null)
        {
            destroyedEnemyCountScript = FindObjectOfType<DestroyedEnemyCount>();
        }
    }

    private void Update()
    {
        if (destroyedEnemyCountScript != null && scoreText != null)
        {
            scoreText.text = "Your score was: " + destroyedEnemyCountScript.DestroyedEnemyCountNumber.ToString();
        }
    }
}
