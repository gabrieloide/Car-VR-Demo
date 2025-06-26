using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using Code;
using UnityEngine.Serialization;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;

public class SteeringWheelRightControl : InteractableButton
{
    [SerializeField] private WiperManager wiperManager; // Referencia al nuevo WiperManager

    private Coroutine _joystickCheckRoutine;

    public bool IsInteracting { get; private set; } = false;

    public override void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        IsInteracting = true;
        _joystickCheckRoutine = StartCoroutine(CheckJoystickDirection());
    }

    public override void OnExitInteract(SelectExitEventArgs selectExitEventArgs)
    {
        IsInteracting = false;
        if (_joystickCheckRoutine != null)
            StopCoroutine(_joystickCheckRoutine);
    }

    private IEnumerator CheckJoystickDirection()
    {
        var rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool buttonAPressed = false;
        bool buttonBPressed = false;
        bool canPress = true;
        

        while (true)
        {
            if (canPress && IsInteracting) // Añadida verificación IsInteracting
            {
                // Detectar botón A
                if (rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonAValue))
                {
                    if (buttonAValue && !buttonAPressed)
                    {
                        wiperManager.ChangeWiperLevel(-1);
                        canPress = false;
                        StartCoroutine(ResetPress());
                        buttonAPressed = true;
                    }
                    else if (!buttonAValue)
                    {
                        buttonAPressed = false;
                    }
                }

                // Detectar botón B
                if (rightHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool buttonBValue))
                {
                    if (buttonBValue && !buttonBPressed)
                    {
                        wiperManager.ChangeWiperLevel(1);
                        canPress = false;
                        StartCoroutine(ResetPress());
                        buttonBPressed = true;
                    }
                    else if (!buttonBValue)
                    {
                        buttonBPressed = false;
                    }
                }
            }

            yield return null;
        }
        IEnumerator ResetPress()
        {
            yield return new WaitForSeconds(0.2f); // Delay de 200ms
            canPress = true;
        }
    }
}