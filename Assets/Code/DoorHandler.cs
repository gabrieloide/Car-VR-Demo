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
    [SerializeField] private float resetSpeed = 5f;
    [SerializeField] private bool mustReset = false;
    [SerializeField] private float maxDistance = 10f;
    private float distance;

    private void OnEnable()
    {
        initialPosition = transform.rotation;
    }

    private void Update()
    {
        if (!TouchingInteractable && mustReset)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, initialPosition, resetSpeed * Time.deltaTime);
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
    }

private IEnumerator UseHandler(Transform controllerPosition)
{
    Vector3 initialControllerPosition = controllerPosition.position;
    
    float rotationOffset = (orientation == Orientation.Up || orientation == Orientation.Down) 
        ? transform.rotation.eulerAngles.y 
        : 0f;

    while (TouchingInteractable)
    {
        Vector3 handlerToController = controllerPosition.position - initialControllerPosition;
        
        float movement = (orientation == Orientation.Up || orientation == Orientation.Down) 
            ? handlerToController.y : new Vector2(handlerToController.x, handlerToController.z).magnitude;

        distance = Mathf.Clamp(movement, 0, maxDistance);
        
        Vector3 targetRotation = RotateOrientation(distance);
        if (orientation == Orientation.Up || orientation == Orientation.Down)
        {
            targetRotation.y += rotationOffset;
        }
        
        transform.rotation = Quaternion.Euler(targetRotation);

        yield return null;
    }

    TouchingInteractable = false;
}

    private Vector3 RotateOrientation(float handlerToController)
    {
        float multiplier = (orientation is Orientation.Up or Orientation.Down) ? 200f : 100f;
        Vector3 rotate = orientation switch
        {
            Orientation.Up => new Vector3(-1 * handlerToController * multiplier , transform.eulerAngles.y, transform.eulerAngles.z),
            Orientation.Down => new Vector3(handlerToController * multiplier, transform.eulerAngles.y, transform.eulerAngles.z),
            Orientation.Right => new Vector3(transform.eulerAngles.x, handlerToController * multiplier, transform.eulerAngles.z),
            Orientation.Left => new Vector3(transform.eulerAngles.x, -1 * handlerToController * multiplier, transform.eulerAngles.z),
            _ => Vector3.zero
        };
        return rotate;    
    }
}
