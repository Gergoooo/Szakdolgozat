using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestroyedEnemyCount : EnemySpawner
{
    public TextMeshProUGUI destroyedEnemyCountText;
    private int destroyedEnemyCount = 0;

    public int DestroyedEnemyCountNumber
    {
        get { return destroyedEnemyCount; }
    }

    private void Start()
    {
        if (destroyedEnemyCountText != null)
        {
            destroyedEnemyCountText.gameObject.SetActive(true);
        }
    }

    public override void HandleEnemyDestroyed(GameObject destroyedEnemy)
    {
        destroyedEnemyCount++;
        UpdateDestroyedEnemyCountText();

        base.HandleEnemyDestroyed(destroyedEnemy);
    }

    private void UpdateDestroyedEnemyCountText()
    {
        if (destroyedEnemyCountText != null)
        {
            destroyedEnemyCountText.text = "Destroyed Enemies: " + destroyedEnemyCount.ToString();
        }
    }
}
