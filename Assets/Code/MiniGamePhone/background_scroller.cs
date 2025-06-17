using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float scrollSpeed = 100f;
    public float resetPositionX = -800f;
    public float startPositionX = 800f;
    
    private RectTransform rectTransform;
    private GameManager gameManager;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        gameManager = FindObjectOfType<GameManager>();
    }
    
    void Update()
    {
        if (gameManager.IsGameOver()) return;
        
        // Mover el fondo
        float currentSpeed = scrollSpeed * gameManager.GetSpeedMultiplier();
        Vector2 position = rectTransform.anchoredPosition;
        position.x -= currentSpeed * Time.deltaTime;
        
        // Resetear posición cuando sale de pantalla
        if (position.x <= resetPositionX)
        {
            position.x = startPositionX;
        }
        
        rectTransform.anchoredPosition = position;
    }
}