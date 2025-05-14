using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class InteractableButton : MonoBehaviour
{
    public abstract void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs);
    public abstract void OnExitInteract(SelectEnterEventArgs selectEnterEventArgs);
}