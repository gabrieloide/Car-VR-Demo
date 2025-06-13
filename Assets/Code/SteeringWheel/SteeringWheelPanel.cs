using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SteeringWheelPanel : MonoBehaviour
{
    protected VRInputAction _inputAction;


    private void Awake()
    {
        _inputAction = new VRInputAction();
        
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        _inputAction.Disable();
    }
}
