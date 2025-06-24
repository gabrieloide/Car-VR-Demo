using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Code.MiniGamePhone
{
    public class Phone : InteractableButton
    {
        [SerializeField] private float rotationSpeed = 5f;
        private bool _wasPrimaryButtonPressedLastFrame = false;
        private bool _wasSecondaryButtonPressedLastFrame = false;
        public bool isHoldingPhone = false;
        private Transform cameraTransform;
        [SerializeField] private GameObject startScreen;
        [SerializeField] private TMP_Text startText;
        
        public override void OnEnterInteract(SelectEnterEventArgs args)
        {
            isHoldingPhone = true;
            cameraTransform = Camera.main.transform;
            string xText ="";
            if (args.interactorObject is XRBaseInteractor baseInteractor)
            {
                string parentName = baseInteractor.transform.parent.name;

                InputDevice handSide = default;
                if (parentName.Contains("Left") || parentName.Contains("Izq"))
                {
                    handSide = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                    xText = "X";
                    Debug.Log("Controlador IZQUIERDO");
                }
                else if (parentName.Contains("Right") || parentName.Contains("Der"))
                {
                    handSide =  InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                    xText = "A";
                    Debug.Log("Controlador DERECHO");
                }
                
                StartCoroutine(CheckStartGame(xText, handSide));
            }

        }

        public override void OnExitInteract(SelectExitEventArgs args)
        {
            isHoldingPhone = false;
            StopCoroutine(CheckStartGame());
        }


        private IEnumerator CheckStartGame(string primaryButton = "", InputDevice handSide = default)
        {
            startText.text = $"Press {primaryButton} to Start";
            var gameStatus = FindAnyObjectByType<GameManager>();
            var player = FindAnyObjectByType<DinosaurController>();
            float xOffset = 15f;
    
            while (isHoldingPhone)
            {
                handSide.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
                handSide.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);

                if (!isHoldingPhone)
                    yield break;
                
                player.isDucking = false;
                if (primaryButtonValue && !_wasPrimaryButtonPressedLastFrame)
                {
                    Debug.Log("Botón presionado agacharse");
                    player.isDucking = true;
                    if (!gameStatus.hasStarted)
                    {
                        startScreen.SetActive(false);
                        gameStatus.hasStarted = true;
                        gameStatus.RestartGame();
                        Debug.Log("El juego no había comenzado. Iniciando ahora.");
                    }
                }
                if (secondaryButtonValue && !_wasSecondaryButtonPressedLastFrame)
                {
                    player.Jump();
                    Debug.Log("Botón presionado saltar");
                }
                
                _wasPrimaryButtonPressedLastFrame = primaryButtonValue;
                _wasSecondaryButtonPressedLastFrame = secondaryButtonValue;
                
                Vector3 direction = cameraTransform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
        
                targetRotation *= Quaternion.Euler(xOffset, 0, 0);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime * rotationSpeed);
        
                yield return null;
            }
            
            _wasPrimaryButtonPressedLastFrame = false;
            _wasSecondaryButtonPressedLastFrame = false;
        }
    }
}