using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PowerWindowSwitch : InteractableButton
{
    private bool _touchingSwitch = false;
    [SerializeField] private GameObject _window;
    [SerializeField] private float threshold = 45;
    [SerializeField] private float windowSpeed = 1;

    private void Start()
    {
        if (_window == null)
            throw new Exception("Window not assigned");

        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(OnEnterInteract);
    }

    public override void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        if (FindAnyObjectByType<WindowBlocker>().isBlocked)
            return;

        _touchingSwitch = true;

        var interactor = selectEnterEventArgs.interactorObject;

        if (interactor is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor controllerInteractor)
        {
            var controller = controllerInteractor.xrController;

            if (controller != null)
            {
                Debug.Log("Controlador interactuando: " + controller.name);
                var currentHand = controller.name.Contains("Left") ? XRNode.LeftHand : XRNode.RightHand;
                //currentHand = controller.controllerNode;
                StartCoroutine(ChangeWindowPosition(currentHand));
            }
        }

        Debug.Log(_touchingSwitch);
    }

    public override void OnExitInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        _touchingSwitch = false;
        Debug.Log(_touchingSwitch);
    }

    private IEnumerator ChangeWindowPosition(XRNode currentHand)
    {
        float currentRotation = 0;

        while (_touchingSwitch)
        {
            var controllerInteraction = InputDevices.GetDeviceAtXRNode(currentHand);

            if (controllerInteraction.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
            {
                Debug.Log("Rotación del control derecho: " + rotation.eulerAngles);

                float direction = currentRotation > threshold ? 1 :
                    currentRotation < -threshold ? -1 : 0;

                _window.transform.position += Vector3.up * (direction * (Mathf.Abs(windowSpeed) * Time.deltaTime));
                Debug.Log($"The transform of the window is {_window.transform.position}");
            }


            yield return null;
        }
    }
}