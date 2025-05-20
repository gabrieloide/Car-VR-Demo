using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WindowBlocker : InteractableButton
{
    public bool isBlocked = false;
    public float windowSpeed = 0.65f;
    public override void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        _audioSource.Play();
        isBlocked = !isBlocked;
    }

    public override void OnExitInteract(SelectExitEventArgs selectEnterEventArgs)
    {
        
    }
}
