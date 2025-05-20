using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PowerWindowSwitch : InteractableButton
{
    [SerializeField] private GameObject _window;
    [SerializeField] private float threshold = 45;
    public Transform initialPosition; 
    public Transform finalPosition; 
    private float _windowSpeed;


    public override void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        _audioSource.Play();
        _windowSpeed = FindAnyObjectByType<WindowBlocker>().windowSpeed;
        if (FindAnyObjectByType<WindowBlocker>().isBlocked)
        {
            Debug.Log("Window is blocked");
            return;
        }

        TouchingInteractable = true;

        StartCoroutine(ChangeWindowPosition(selectEnterEventArgs.interactorObject.transform));
    }

    public override void OnExitInteract(SelectExitEventArgs selectEnterEventArgs)
    {
        TouchingInteractable = false;
    }

    
    

    private IEnumerator ChangeWindowPosition(Transform controllerInteraction)
    {
        float initialControllerRotation = controllerInteraction.eulerAngles.x;
        float activationThreshold = 15f; // Ángulo mínimo para activar movimiento
        bool isMoving = false;

        while (TouchingInteractable)
        {
            // Calcular diferencia angular
            float currentRotation = controllerInteraction.eulerAngles.x;
            float delta = Mathf.DeltaAngle(initialControllerRotation, currentRotation);

            // Determinar dirección
            if (Mathf.Abs(delta) > activationThreshold)
            {
                Vector3 targetPosition = delta > 0 ? finalPosition.position : initialPosition.position;
            
                // Mover hacia la posición objetivo con velocidad fija
                _window.transform.position = Vector3.MoveTowards(
                    _window.transform.position,
                    targetPosition,
                    _windowSpeed * Time.deltaTime
                );
            
                isMoving = true;
            }
            else if (isMoving)
            {
                // Opcional: Resetear posición si se necesita volver al centro
                // _window.transform.position = Vector3.MoveTowards(...);
                isMoving = false;
            }

            yield return null;
        }
    }
}