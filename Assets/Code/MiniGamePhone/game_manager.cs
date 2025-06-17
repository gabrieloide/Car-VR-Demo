using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public GameObject gameOverPanel;
    public Button restartButton;
    
    [Header("Game Settings")]
    public float scoreMultiplier = 10f;
    public float speedIncreaseRate = 0.01f;
    public float maxSpeedMultiplier = 3f;
    
    private float score = 0f;
    private float highScore;
    private bool gameOver = false;
    private float gameStartTime;
    private float baseSpeedMultiplier = 1f;
    
    private DinosaurController dinosaur;
    private ObstacleSpawner obstacleSpawner;
    
    void Start()
    {
        highScore = PlayerPrefs.GetFloat("DinoHighScore", 0f);
        
        dinosaur = FindObjectOfType<DinosaurController>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        //StartGame();
    }
    private void OnDrawGizmos()
    {
        if (dinosaur == null)
            return;
        
        var dinoSize = new Vector2(0.00621f, 0.00639f);
        var dinoPos = FindAnyObjectByType<DinosaurController>().transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(dinoPos, dinoSize);
        var obstacles = FindObjectsByType<ObstacleController>(FindObjectsSortMode.None);
        
        if (obstacleSpawner == null)
            return;
        
        Gizmos.color = Color.red;
        foreach (var obstacle in obstacles)
        {
            var obstaclePos = obstacle.transform.position;
            float obstacleWidth = 40f;
            Vector2 obstacleHeight = obstacle.isBird ? new Vector2(0.00621f, 0.00639f/2.3f) : new Vector2(0.00621f/2.3f, 0.00639f);
            
            Vector2 obstacleSize = obstacleHeight;
            Gizmos.DrawWireCube(obstaclePos, obstacleSize);
        }

    }
    void Update()
    {
        if (!gameOver)
        {
            UpdateScore();
            UpdateSpeedMultiplier();
            UpdateUI();
        }
        
        // Reiniciar con espacio cuando el juego termina
        //if (gameOver && Input.GetKeyDown(KeyCode.Space))
        //{
        //    RestartGame();
        //}
    }
    
    void StartGame()
    {
        gameOver = false;
        score = 0f;
        baseSpeedMultiplier = 1f;
        gameStartTime = Time.time;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        UpdateUI();
    }
    
    void UpdateScore()
    {
        score += scoreMultiplier * Time.deltaTime;
    }
    
    void UpdateSpeedMultiplier()
    {
        float timeElapsed = Time.time - gameStartTime;
        baseSpeedMultiplier = Mathf.Min(1f + (timeElapsed * speedIncreaseRate), maxSpeedMultiplier);
    }
    
    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString("00000");
        
        if (highScoreText != null)
            highScoreText.text = "HI: " + Mathf.FloorToInt(highScore).ToString("00000");
    }
    
    public void GameOver()
    {
        if (gameOver) return;
        
        gameOver = true;
        
        // Actualizar high score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("DinoHighScore", highScore);
            PlayerPrefs.Save();
        }
        
        // Mostrar panel de game over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        
        UpdateUI();
    }
    
    public void RestartGame()
    {
        // Limpiar obstáculos
        if (obstacleSpawner != null)
            obstacleSpawner.ClearAllObstacles();
        
        // Resetear dinosaurio
        if (dinosaur != null)
            dinosaur.ResetPosition();
        
        StartGame();
    }
    
    public bool IsGameOver()
    {
        return gameOver;
    }
    
    public float GetSpeedMultiplier()
    {
        return baseSpeedMultiplier;
    }
    
    public float GetScore()
    {
        return score;
    }
}