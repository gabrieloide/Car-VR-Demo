using UnityEngine;
using UnityEngine.InputSystem;

public class SteeringWheelPanel : MonoBehaviour
{
    private bool _isTouchingSteeringWheel => GetComponent<SteeringWheel>().isPressed ;
    private  VRInputAction _inputAction;
    [SerializeField] private GameObject wiper;

    private void Awake()
    {
        _inputAction = new VRInputAction();
        _inputAction.Player.SteeringWheelLeftButtons.performed += LeftStick;
        _inputAction.Player.SteeringWheelRightButtons.performed += RightStick;
    }
    private void OnEnable()
    {
        _inputAction.Enable();
    }
    private void OnDisable()
    {
        _inputAction.Disable();
    }
    private void LeftStick(InputAction.CallbackContext context)
    {
        var direction =context.ReadValue<Vector2>();

        if(!_isTouchingSteeringWheel) return;

        if(direction.x > 0.5f)
        {
            Debug.Log("Right");
        }
        else if(direction.x < -0.5f)
        {
            Debug.Log("Left");
        }
        else if(direction.y > 0.5f)
        {
            Debug.Log("Up");
        }
        else if(direction.y < -0.5f)
        {
            Debug.Log("Down");
        }
    }
    private void RightStick(InputAction.CallbackContext context)
    {
        var direction =context.ReadValue<Vector2>();

        if(!_isTouchingSteeringWheel) return;

        if(direction.x > 0.5f)
        {
            Debug.Log("Right");
        }
        else if(direction.x < -0.5f)
        {
            Debug.Log("Left");
        }
        else if(direction.y > 0.5f)
        {
            Debug.Log("Up");
        }
        else if(direction.y < -0.5f)
        {
            Debug.Log("Down");
        }
    }
    public virtual void DoSomething()
    {
        Debug.Log("DoSomething");
    }

}
