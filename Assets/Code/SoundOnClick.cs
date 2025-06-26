using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SoundOnClick : InteractableButton
{
    public override void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        _audioSource.Play();
    }

    public override void OnExitInteract(SelectExitEventArgs selectEnterEventArgs)
    {
    }
}
