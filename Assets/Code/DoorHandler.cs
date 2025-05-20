using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using Unity.Mathematics;

public class DoorHandler : InteractableButton
{
    private Quaternion initialPosition;

    private void OnEnable()
    {
        initialPosition = transform.rotation;
    }

    private void Update()
    {
        if (!TouchingInteractable)
        {
            transform.rotation = initialPosition;
        }
    }

    public override void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        TouchingInteractable = true;
        StartCoroutine(UseHandler(selectEnterEventArgs.interactorObject.transform));
    }

    public override void OnExitInteract(SelectExitEventArgs selectEnterEventArgs)
    {
        TouchingInteractable = false;
        StopCoroutine(UseHandler(selectEnterEventArgs.interactorObject.transform));
        transform.rotation = initialPosition;
    }

    private IEnumerator UseHandler(Transform controllerPosition)
    {
        while (TouchingInteractable)
        {
            var handlerToController = (controllerPosition.position - transform.position).magnitude;
            transform.eulerAngles = Quaternion.Euler(0, handlerToController * 100, 0).eulerAngles;
            yield return null;
        }
        TouchingInteractable = false;
    }
}