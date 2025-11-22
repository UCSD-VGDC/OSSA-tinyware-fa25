using UnityEngine;

public class RerollButton : CustomButton
{
    [SerializeField] private Color disabledColor;
    private Color originalColor;

    protected override void OnEnable()
    {
        base.OnEnable();
        originalColor = image.color;
    }

    public void ToggleEnabled(bool isEnabled)
    {
        image.color = isEnabled ? originalColor : disabledColor;
    }
}
