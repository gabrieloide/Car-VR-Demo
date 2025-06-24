using System;
using UnityEngine;
using UnityEngine.UI;

public class DinosaurController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 500f;
    public float gravity = -25f;
    public float groundY = -200f;
    
    [Header("Animation")]
    public Sprite[] runSprites;
    public Sprite jumpSprite;
    public Sprite duckSprite;
    public float animationSpeed = 0.1f;
    
    private RectTransform rectTransform;
    private Image image;
    private float verticalVelocity = 0f;
    private bool isGrounded = true;
    public bool isDucking = false;
    private int currentFrame = 0;
    private float animationTimer = 0f;
    private GameManager gameManager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        
        image = GetComponent<Image>();
        gameManager = FindObjectOfType<GameManager>();
        
        rectTransform.anchoredPosition = new Vector2(-0.02519f, groundY);
    }
    
    void Update()
    {
        if (gameManager.IsGameOver()) return;
        
        HandleInput();
        HandleMovement();
        HandleAnimation();
    }



    void HandleInput()
    {
        // Saltar
        //if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        //{
        //    Jump();
        //}
        
        // Agacharse
        isDucking = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
    }
    
    void HandleMovement()
    {
        // Aplicar gravedad
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
            Vector2 pos = rectTransform.anchoredPosition;
            pos.y += verticalVelocity * Time.deltaTime;
            
            // Verificar si toca el suelo
            if (pos.y <= groundY)
            {
                pos.y = groundY;
                verticalVelocity = 0f;
                isGrounded = true;
            }
            
            rectTransform.anchoredPosition = pos;
        }
        
        // Ajustar altura cuando se agacha
        if (isDucking && isGrounded)
        {
            Vector2 pos = rectTransform.anchoredPosition;
            pos.y = groundY - 20f;
            rectTransform.anchoredPosition = pos;
        }
        else if (isGrounded)
        {
            Vector2 pos = rectTransform.anchoredPosition;
            pos.y = groundY;
            rectTransform.anchoredPosition = pos;
        }
    }
    
    void HandleAnimation()
    {
        animationTimer += Time.deltaTime;
        
        if (!isGrounded)
        {
            // Sprite de salto
            image.sprite = jumpSprite;
        }
        else if (isDucking)
        {
            // Sprite agachado
            image.sprite = duckSprite;
        }
        else
        {
            // Animación de correr
            if (animationTimer >= animationSpeed)
            {
                currentFrame = (currentFrame + 1) % runSprites.Length;
                image.sprite = runSprites[currentFrame];
                animationTimer = 0f;
            }
        }
    }
    
    public void Jump()
    {
        verticalVelocity = jumpForce;
        isGrounded = false;
    }
    
    public bool IsDucking()
    {
        return isDucking;
    }
    
    public Vector2 GetPosition()
    {
        return rectTransform.anchoredPosition;
    }
    
    public void ResetPosition()
    {
        rectTransform.anchoredPosition = new Vector2(-300f, groundY);
        verticalVelocity = 0f;
        isGrounded = true;
        isDucking = false;
    }
}