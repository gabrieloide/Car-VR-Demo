using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SteeringWheel : InteractableButton
{
    public XRNode leftHandNode = XRNode.LeftHand;
    public XRNode rightHandNode = XRNode.RightHand;

    public float maxRotation = 90f;
    public float smoothing = 10f;

    private float currentZRotation = 0f;
    public bool isPressed => TouchingInteractable;
    
    public override void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        TouchingInteractable = true;
        StartCoroutine(RotateSteeringWheel());
    }
    public override void OnExitInteract(SelectExitEventArgs selectEnterEventArgs)
    {
        TouchingInteractable = false;
    }

    IEnumerator RotateSteeringWheel()
    {
        while (TouchingInteractable)
        {
            var leftDevice = InputDevices.GetDeviceAtXRNode(leftHandNode);
            var rightDevice = InputDevices.GetDeviceAtXRNode(rightHandNode);

            if (leftDevice.TryGetFeatureValue(CommonUsages.devicePosition, out var leftPos) &&
                rightDevice.TryGetFeatureValue(CommonUsages.devicePosition, out var rightPos))
            {
                Vector3 handDirection = rightPos - leftPos;
                
                Vector3 flatDirection = Vector3.ProjectOnPlane(handDirection, Vector3.forward);
                
                float angle = Vector3.SignedAngle(Vector3.right, flatDirection, Vector3.forward);
                
                angle = Mathf.Clamp(angle, -maxRotation, maxRotation);
                currentZRotation = Mathf.Lerp(currentZRotation, angle, Time.deltaTime * smoothing);

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentZRotation);
            }
            yield return null;
        }
    }
}
