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
    [FormerlySerializedAs("wiper")] [SerializeField]
    private GameObject wiperHandler;

    private Coroutine _joystickCheckRoutine;

    public override void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        _joystickCheckRoutine = StartCoroutine(CheckJoystickDirection());
    }

    public override void OnExitInteract(SelectExitEventArgs selectEnterEventArgs)
    {
        if (_joystickCheckRoutine != null)
            StopCoroutine(_joystickCheckRoutine);
    }

    private IEnumerator CheckJoystickDirection()
    {
        var rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool buttonAPressed = false;
        bool buttonBPressed = false;
        bool canPress = true;
        var wiper = FindObjectsByType<Wiper>(FindObjectsSortMode.None);
        

        while (true)
        {
            if (canPress)
            {
                // Detectar botón A
                if (rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonAValue))
                {
                    if (buttonAValue && !buttonAPressed)
                    {
                        if (wiper[0].wiperLevel > 0 & wiper[0].wiperLevel > 0)
                        {
                            MoveWiper(1);
                            wiper[0].wiperLevel--;
                            wiper[1].wiperLevel--;
                            canPress = false;
                            StartCoroutine(ResetPress());
                        }
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
                        if (wiper[0].wiperLevel <= 4 && wiper[1].wiperLevel <= 4)
                        {
                            MoveWiper(-1);
                            wiper[0].wiperLevel++;
                            wiper[1].wiperLevel++;
                            canPress = false;
                            StartCoroutine(ResetPress());
                        }
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

    private void MoveWiper(float direction)
    {
        float currentRotation = wiperHandler.transform.rotation.eulerAngles.x;
        wiperHandler.transform.DORotate(
            new Vector3(currentRotation + direction * 10, wiperHandler.transform.rotation.eulerAngles.y,
                wiperHandler.transform.rotation.eulerAngles.z), 0.5f);
    }
}