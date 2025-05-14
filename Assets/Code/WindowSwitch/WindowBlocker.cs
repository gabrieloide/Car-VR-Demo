using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WindowBlocker : InteractableButton
{
    public bool isBlocked = false;
    public override void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        isBlocked = !isBlocked;
    }

    public override void OnExitInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        
    }
}
