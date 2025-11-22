using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    public static event Action<InputController> OnButtonLeftPressedEvent;
    public static event Action<InputController> OnButtonRightPressedEvent;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    private void OnEnable()
    {
        inputActions.Player.ButtonL.performed += ctx => OnButtonLeft();
        inputActions.Player.ButtonR.performed += ctx => OnButtonRight();
    }

    private void OnDisable()
    {
        inputActions.Player.ButtonL.performed -= ctx => OnButtonLeft();
        inputActions.Player.ButtonR.performed -= ctx => OnButtonRight();
    }

    private void OnButtonLeft()
    {
        OnButtonLeftPressedEvent?.Invoke(this);
    }

    private void OnButtonRight()
    {
        OnButtonRightPressedEvent?.Invoke(this);
    }
    
    private void OnDestroy()
    {
        inputActions.Player.Disable();
    }
}
