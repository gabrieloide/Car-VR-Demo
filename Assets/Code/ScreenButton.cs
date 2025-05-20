using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScreenButton : InteractableButton
{
    [SerializeField] private GameObject screenToActivate;
    
    public override void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs)
    {
        _audioSource.Play();
        if (screenToActivate == null) return;
        
        Screen.Instance.currentActiveScreen.SetActive(false);
        screenToActivate.SetActive(true);
        Screen.Instance.currentActiveScreen = screenToActivate;
    }

    public override void OnExitInteract(SelectExitEventArgs selectEnterEventArgs)
    {
    }
}

