using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    public enum ButtonType
    {
        Left,
        Right,
        Both
    }

    public ButtonType buttonType;
    [SerializeField] protected Image image;
    [SerializeField] private Sprite releasedSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private RectTransform shiftedElement;
    private Vector2 originalPosition = new(-1, -1);

    protected virtual void OnEnable()
    {
        switch (buttonType)
        {
            case ButtonType.Left:
                InputController.OnButtonLeftPressedEvent += HandleButtonPressed;
                InputController.OnButtonLeftReleasedEvent += HandleButtonReleased;
                break;
            case ButtonType.Right:
                InputController.OnButtonRightPressedEvent += HandleButtonPressed;
                InputController.OnButtonRightReleasedEvent += HandleButtonReleased;
                break;
            case ButtonType.Both:
                InputController.OnBothButtonsPressedEvent += HandleButtonPressed;
                InputController.OnBothButtonsReleasedEvent += HandleButtonReleased;
                break;
        }

        if (originalPosition == new Vector2(-1, -1))
        {
            originalPosition = shiftedElement.anchoredPosition;
        }
        TogglePressed(false);
    }

    private void HandleButtonPressed(InputController controller) { TogglePressed(true); }
    private void HandleButtonReleased(InputController controller) { TogglePressed(false); }

    private void TogglePressed(bool isPressed)
    {
        if (isPressed)
        {
            image.sprite = pressedSprite;
            if (shiftedElement != null)
            {
                shiftedElement.anchoredPosition = originalPosition - new Vector2(0, 1);
            }
        }
        else
        {
            image.sprite = releasedSprite;
            if (shiftedElement != null)
            {
                shiftedElement.anchoredPosition = originalPosition;
            }
        }
    }
}
