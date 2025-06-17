using UnityEngine;
using UnityEngine.UI;

public class ObstacleController : MonoBehaviour
{
    [Header("Obstacle Settings")] public float baseSpeed = 300f;
    public bool isBird = false;

    [Header("Bird Animation (solo para pájaros)")]
    public Sprite[] birdSprites;

    public float animationSpeed = 0.2f;

    private RectTransform rectTransform;
    private Image image;
    private GameManager gameManager;
    private DinosaurController dinosaur;
    

    // Para animación del pájaro
    private int currentFrame = 0;
    private float animationTimer = 0f;

    void Start()
    {
        
        
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        gameManager = FindObjectOfType<GameManager>();
        dinosaur = FindObjectOfType<DinosaurController>();
    }

    void Update()
    {
        if (gameManager.IsGameOver()) return;

        MoveObstacle();
        CheckCollision();

        if (isBird && birdSprites.Length > 0)
        {
            AnimateBird();
        }
    }

    void MoveObstacle()
    {
        float currentSpeed = baseSpeed * gameManager.GetSpeedMultiplier();
        Vector2 position = rectTransform.anchoredPosition;
        position.x -= currentSpeed * Time.deltaTime;
        rectTransform.anchoredPosition = position;
    }

    void CheckCollision()
    {
        Vector2 dinoPos = dinosaur.GetPosition();
        Vector2 obstaclePos = rectTransform.anchoredPosition;

        // Definir áreas de colisión
        float dinoHeight = dinosaur.IsDucking() ? 0.00639f : 0.00639f/2;

        // Verificar colisión usando AABB (Axis-Aligned Bounding Box)
        var dinoSize = new Vector2(0.00621f, dinoHeight);
        Vector2 obstacleSize = isBird ? new Vector2(0.00621f, 0.00639f/2.3f) : new Vector2(0.00621f/2.3f, 0.00639f);

        bool collisionX = Mathf.Abs(dinoPos.x - obstaclePos.x) < (dinoSize.x + obstacleSize.x) / 2f;
        bool collisionY = Mathf.Abs(dinoPos.y - obstaclePos.y) < (dinoSize.y + obstacleSize.y) / 2f;

        if (collisionX && collisionY)
        {
            gameManager.GameOver();
        }
    }

    private void OnDrawGizmos()
    {


    }

    void AnimateBird()
    {
        animationTimer += Time.deltaTime;

        if (animationTimer >= animationSpeed)
        {
            currentFrame = (currentFrame + 1) % birdSprites.Length;
            image.sprite = birdSprites[currentFrame];
            animationTimer = 0f;
        }
    }
}