using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public enum Orientation
{
    Left,
    Right,
    Up,
    Down
}

public class DoorHandler : InteractableButton
{
    private Quaternion initialPosition;
    [SerializeField] private float resetSpeed = 5f;
    private float distance;

    private void OnEnable()
    {
        initialPosition = transform.rotation;
    }

    public override void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        touchingInteractable = true;
        StartCoroutine(UseHandler(selectEnterEventArgs.interactorObject.transform));
    }

    public override void OnExitInteract(SelectExitEventArgs selectEnterEventArgs)
    {
        touchingInteractable = false;
        StopCoroutine(UseHandler(selectEnterEventArgs.interactorObject.transform));
    }

   [Header("Configuración de Interacción")]
    public Orientation orientation;
    public float maxDistance = 1f;

    [Header("Configuración de Reinicio")]
    [Tooltip("Si está activado, el objeto volverá a su rotación inicial al soltarlo.")]
    public bool mustReset = true; // <-- NUEVA VARIABLE
    [Tooltip("La duración en segundos de la animación de reinicio.")]
    public float resetDuration = 0.5f; // <-- NUEVA VARIABLE

    public bool touchingInteractable { get; private set; }

    [Tooltip("Distancia de rotación aplicada actualmente. Se reinicia si 'mustReset' es verdadero.")]
    public float currentAppliedDistance = 0f;

    private Quaternion baseRotation;

    // --- NUEVA VARIABLE PARA GESTIONAR LA CORRUTINA DE REINICIO ---
    private Coroutine resetCoroutine;

    private void Awake()
    {
        baseRotation = transform.rotation;
    }

    private IEnumerator UseHandler(Transform controllerPosition)
    {
        touchingInteractable = true;

        // --- MANEJO DE INTERRUPCIONES ---
        // Si el objeto se estaba reiniciando, detenemos esa animación inmediatamente.
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }
        
        Vector3 lastControllerPosition = controllerPosition.position;

        while (touchingInteractable)
        {
            Vector3 controllerDelta = controllerPosition.position - lastControllerPosition;

            float movementDelta;
            if (orientation == Orientation.Up)
            {
                movementDelta = controllerDelta.y;
            }
            else if (orientation == Orientation.Down)
            {
                movementDelta = -controllerDelta.y;
            }
            else
            {
                movementDelta = controllerDelta.x;
            }

            currentAppliedDistance += movementDelta;
            currentAppliedDistance = Mathf.Clamp(currentAppliedDistance, 0, maxDistance);
            transform.rotation = CalculateTargetRotation(currentAppliedDistance);
            lastControllerPosition = controllerPosition.position;

            yield return null;
        }

        // --- LÓGICA DE REINICIO AL FINALIZAR LA INTERACCIÓN ---
        // Si la opción está activada, comenzamos la corrutina de reinicio.
        if (mustReset)
        {
            resetCoroutine = StartCoroutine(ResetRotation());
        }
    }

    /// <summary>
    /// Anima suavemente el objeto de vuelta a su rotación inicial.
    /// </summary>
    private IEnumerator ResetRotation()
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        float startDistance = currentAppliedDistance;

        while (elapsedTime < resetDuration)
        {
            // Calcula el progreso de la animación (de 0 a 1)
            float t = elapsedTime / resetDuration;
            // Interpola suavemente la rotación y el valor de la distancia.
            transform.rotation = Quaternion.Slerp(startRotation, baseRotation, t);
            currentAppliedDistance = Mathf.Lerp(startDistance, 0f, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegura que al final de la animación, los valores sean exactamente los iniciales.
        transform.rotation = baseRotation;
        currentAppliedDistance = 0f;
        resetCoroutine = null; // Libera la referencia a la corrutina.
    }

    private Quaternion CalculateTargetRotation(float totalDistance)
    {
        float multiplier = (orientation == Orientation.Up || orientation == Orientation.Down) ? 200f : 100f;
        float angle = totalDistance * multiplier;

        Quaternion deltaRotation;

        switch (orientation)
        {
            case Orientation.Up:
                deltaRotation = Quaternion.Euler(-angle, 0, 0);
                break;
            case Orientation.Down:
                deltaRotation = Quaternion.Euler(angle, 0, 0);
                break;
            case Orientation.Right:
                deltaRotation = Quaternion.Euler(0, angle, 0);
                break;
            case Orientation.Left:
                deltaRotation = Quaternion.Euler(0, -angle, 0);
                break;
            default:
                deltaRotation = Quaternion.identity;
                break;
        }

        return baseRotation * deltaRotation;
    }
}