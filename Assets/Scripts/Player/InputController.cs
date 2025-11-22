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
    private float chordPressWindow = 0.1f;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    private void OnEnable()
    {
        inputActions.Player.ButtonL.performed += OnButtonLeft;
        inputActions.Player.ButtonR.performed += OnButtonRight;
    }

    private void OnDisable()
    {
        inputActions.Player.ButtonL.performed -= OnButtonLeft;
        inputActions.Player.ButtonR.performed -= OnButtonRight;
    }

    private void OnButtonLeft(InputAction.CallbackContext context)
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

    private void OnButtonRight(InputAction.CallbackContext context)
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
