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
    
    private float _score = 0f;
    private float _highScore;
    private float _gameStartTime;
    private float _baseSpeedMultiplier = 1f;
    private bool  _gameOver = false;
    public bool hasStarted = false;
    
    private DinosaurController _dinosaur;
    private ObstacleSpawner _obstacleSpawner;
    
    void Start()
    {
        _highScore = PlayerPrefs.GetFloat("DinoHighScore", 0f);
        
        _dinosaur = FindAnyObjectByType<DinosaurController>();
        _obstacleSpawner = FindAnyObjectByType<ObstacleSpawner>();
        _gameOver = true;
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        
    }
    private void OnDrawGizmos()
    {
        if (_dinosaur == null)
            return;
        
        var dinoSize = new Vector2(0.00621f, 0.00639f);
        var dinoPos = FindAnyObjectByType<DinosaurController>().transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(dinoPos, dinoSize);
        var obstacles = FindObjectsByType<ObstacleController>(FindObjectsSortMode.None);
        
        if (_obstacleSpawner == null)
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
        if (!_gameOver)
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
    
    public void StartGame()
    {
        _gameOver = false;
        hasStarted = true;
        _score = 0f;
        _baseSpeedMultiplier = 1f;
        _gameStartTime = Time.time;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        UpdateUI();
    }
    
    private void UpdateScore()
    {
        _score += scoreMultiplier * Time.deltaTime;
    }
    
    private void UpdateSpeedMultiplier()
    {
        var timeElapsed = Time.time - _gameStartTime;
        _baseSpeedMultiplier = Mathf.Min(1f + (timeElapsed * speedIncreaseRate), maxSpeedMultiplier);
    }
    
    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(_score).ToString("00000");
        
        if (highScoreText != null)
            highScoreText.text = "HI: " + Mathf.FloorToInt(_highScore).ToString("00000");
    }
    
    public void GameOver()
    {
        if (_gameOver) return;
        
        _gameOver = true;
        
        if (_score > _highScore)
        {
            _highScore = _score;
            PlayerPrefs.SetFloat("DinoHighScore", _highScore);
            PlayerPrefs.Save();
        }
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        
        UpdateUI();
    }
    
    public void RestartGame()
    {
        if (_obstacleSpawner != null)
            _obstacleSpawner.ClearAllObstacles();
        
        if (_dinosaur != null)
            _dinosaur.ResetPosition();
        
        StartGame();
    }
    
    public bool IsGameOver()
    {
        return _gameOver;
    }
    
    public float GetSpeedMultiplier()
    {
        return _baseSpeedMultiplier;
    }
    
    public float GetScore()
    {
        return _score;
    }
}