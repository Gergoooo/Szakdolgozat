using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private Transform player => PlayerPlaneController.Instance;

    public int spawnAmount = 5;
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 50f;
    public float spawnRadius = 500f;
    public float minSpawnDistance = 100f;
    public float minYPosition = 250f;
    public float maxYPosition = 750f;
    public float clearanceAboveTerrain = 50f;
    private int displaycount = 0;
    
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<GameObject> destroyedEnemies = new List<GameObject>();
    public TextMeshProUGUI spawnCountText; 

    private void Awake()
    {
         displaycount = spawnAmount;
    }

    public int DisplayCount
    {
        get => displaycount;
        set
        {
            displaycount = Mathf.Max(0, value);
            UpdateSpawnCountText();
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnObjects());
        UpdateSpawnCountText();
    }

    private IEnumerator SpawnObjects()
    {
        while (spawnedEnemies.Count < spawnAmount)
        {
            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);

            Vector3 spawnPosition;
            do
            {
                spawnPosition = GenerateRandomPosition();
            }
            while (!CheckSpawnPosition(spawnPosition));

            GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(newEnemy);
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        Vector3 randomPosition;
        float distanceFromPlayer;

        do
        {
            randomPosition = player.position + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(minYPosition, maxYPosition),
                Random.Range(-spawnRadius, spawnRadius)
            );

            distanceFromPlayer = Vector3.Distance(new Vector3(randomPosition.x, player.position.y, randomPosition.z), player.position);
        }
        while (distanceFromPlayer < minSpawnDistance || distanceFromPlayer > spawnRadius);

        return randomPosition;
    }

    private bool CheckSpawnPosition(Vector3 position)
    {
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit))
        {
            return hit.point.y + clearanceAboveTerrain <= position.y;
        }
        return true;
    }

    public virtual void HandleEnemyDestroyed(GameObject destroyedEnemy)
    {
        spawnedEnemies.Remove(destroyedEnemy);
        destroyedEnemies.Add(destroyedEnemy);
        displaycount = Mathf.Max(0, displaycount - 1);
        UpdateSpawnCountText();
    }

    private void UpdateSpawnCountText()
    {
        if (spawnCountText != null)
        {
            spawnCountText.text = "Enemies left: " + displaycount.ToString();
        }
    }
}