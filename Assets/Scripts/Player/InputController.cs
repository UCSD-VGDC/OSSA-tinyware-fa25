using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    public static event Action<InputController> OnButtonLeftPressedEvent;
    public static event Action<InputController> OnButtonRightPressedEvent;
    public static event Action<InputController> OnBothButtonsPressedEvent;
    public static event Action<InputController> OnButtonLeftReleasedEvent;
    public static event Action<InputController> OnButtonRightReleasedEvent;
    public static event Action<InputController> OnBothButtonsReleasedEvent;
    private float chordPressWindow = 0.1f;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    private void OnEnable()
    {
        inputActions.Player.ButtonL.performed += OnButtonLeftPerformed;
        inputActions.Player.ButtonR.performed += OnButtonRightPerformed;
        inputActions.Player.ButtonL.canceled += OnButtonLeftReleased;
        inputActions.Player.ButtonR.canceled += OnButtonRightReleased;
    }

    private void OnDisable()
    {
        inputActions.Player.ButtonL.performed -= OnButtonLeftPerformed;
        inputActions.Player.ButtonR.performed -= OnButtonRightPerformed;
        inputActions.Player.ButtonL.canceled -= OnButtonLeftReleased;
        inputActions.Player.ButtonR.canceled -= OnButtonRightReleased;
    }

    private void OnButtonLeftPerformed(InputAction.CallbackContext context)
    {
        if (inputActions.Player.ButtonR.IsPressed())
        {
            OnBothButtonsPressedEvent?.Invoke(this);
        }
        else
        {
            StartCoroutine(SinglePressCheck(OnButtonLeftPressedEvent, inputActions.Player.ButtonR));
        }
    }

    private void OnButtonLeftReleased(InputAction.CallbackContext context)
    {
        OnBothButtonsReleasedEvent?.Invoke(this);
        OnButtonLeftReleasedEvent?.Invoke(this);
    }

    private void OnButtonRightPerformed(InputAction.CallbackContext context)
    {
        if (inputActions.Player.ButtonL.IsPressed())
        {
            OnBothButtonsPressedEvent?.Invoke(this);
        }
        else
        {
            StartCoroutine(SinglePressCheck(OnButtonRightPressedEvent, inputActions.Player.ButtonL));
        }
    }

    private void OnButtonRightReleased(InputAction.CallbackContext context)
    {
        OnBothButtonsReleasedEvent?.Invoke(this);
        OnButtonRightReleasedEvent?.Invoke(this);
    }

    private IEnumerator SinglePressCheck(Action<InputController> pressEvent, InputAction otherButton)
    {
        yield return new WaitForSecondsRealtime(chordPressWindow);
        if (!otherButton.IsPressed())
        {
            pressEvent?.Invoke(this);
        }
    }

    private void OnDestroy()
    {
        inputActions.Player.Disable();
    }
}
