using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject cactusPrefab;
    public GameObject birdPrefab;
    
    [Header("Spawn Settings")]
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 3f;
    public float spawnX = 800f;
    public float cactusY = -200f;
    public float birdY = -100f;
    
    private float nextSpawnTime;
    private GameManager gameManager;
    private List<GameObject> activeObstacles = new List<GameObject>();
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        ScheduleNextSpawn();
    }
    
    void Update()
    {
        if (gameManager.IsGameOver()) return;
        
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            ScheduleNextSpawn();
        }
        
        // Limpiar obstáculos que salieron de pantalla
        CleanupObstacles();
    }
    
    void SpawnObstacle()
    {
        GameObject obstaclePrefab;
        Vector2 offsetSpawnPosition;
        
        // Decidir aleatoriamente entre cactus y pájaro
        if (Random.Range(0f, 1f) < 0.7f) // 70% cactus, 30% pájaro
        {
            obstaclePrefab = cactusPrefab;
            offsetSpawnPosition = new Vector2( spawnX, cactusY);
        }
        else
        {
            obstaclePrefab = birdPrefab;
            offsetSpawnPosition = new Vector2(spawnX, birdY);
        }
        
        GameObject obstacle = Instantiate(obstaclePrefab, transform);
        RectTransform rectTransform = obstacle.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero + offsetSpawnPosition;
        Debug.Log("Spawned obstacle at: " + rectTransform.anchoredPosition);
        
        activeObstacles.Add(obstacle);
    }
    
    void ScheduleNextSpawn()
    {
        float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
        // Reducir el delay conforme aumenta la velocidad del juego
        delay *= (1f / gameManager.GetSpeedMultiplier());
        nextSpawnTime = Time.time + delay;
    }
    
    void CleanupObstacles()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            if (activeObstacles[i] == null)
            {
                activeObstacles.RemoveAt(i);
                continue;
            }
            
            RectTransform rectTransform = activeObstacles[i].GetComponent<RectTransform>();
            if (rectTransform.anchoredPosition.x < -100f)
            {
                Destroy(activeObstacles[i]);
                activeObstacles.RemoveAt(i);
            }
        }
    }
    
    public void ClearAllObstacles()
    {
        foreach (GameObject obstacle in activeObstacles)
        {
            if (obstacle != null)
                Destroy(obstacle);
        }
        activeObstacles.Clear();
    }
}