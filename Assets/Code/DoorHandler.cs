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
    [SerializeField] private Orientation orientation = Orientation.Down;

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
            if (TouchingInteractable == false)
            {
                yield break;
            }

            var handlerToController = controllerPosition.position - transform.position;
            float distance = (orientation == Orientation.Up || orientation == Orientation.Down) 
                ? handlerToController.y 
                : handlerToController.magnitude;
            
            transform.eulerAngles = Quaternion.Euler(RotateOrientation(distance)).eulerAngles;

            yield return null;
        }

        TouchingInteractable = false;
    }

    private Vector3 RotateOrientation(float handlerToController)
    {
        Vector3 rotate = orientation switch
        {
            Orientation.Up => Vector3.right,
            Orientation.Down => Vector3.left,
            Orientation.Right => Vector3.up,
            Orientation.Left => Vector3.down,
            _ => Vector3.zero
        };

        float multiplier = (orientation is Orientation.Up or Orientation.Down) ? 200f : 100f;
        return rotate * handlerToController * multiplier;
    }
}