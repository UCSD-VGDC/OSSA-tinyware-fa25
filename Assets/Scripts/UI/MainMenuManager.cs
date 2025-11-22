using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public enum MenuState
    {
        Main,
        Credits,
        NoInput
    }

    [SerializeField] private GameObject titleScreenUI;
    [SerializeField] private GameObject creditsUI;
    [SerializeField] private Image blackOverlay;
    private MenuState CurrentState = MenuState.NoInput;

    private void Start()
    {
        StartCoroutine(Tweens.InterpolateRealTime(
            null,
            (t) =>
            {
                float newAlpha = Tweens.EaseInOutCubic(1f, 0f, t);
                blackOverlay.color = new Color(0, 0, 0, newAlpha);
            },
            () => { ShowTitleScreen(); },
            1f
        ));
    }

    private void OnEnable()
    {
        InputController.OnButtonLeftPressedEvent += MenuButtonLeft;
        InputController.OnButtonRightPressedEvent += MenuButtonRight;
        InputController.OnBothButtonsPressedEvent += MenuButtonBoth;
    }

    private void OnDisable()
    {
        InputController.OnButtonLeftPressedEvent -= MenuButtonLeft;
        InputController.OnButtonRightPressedEvent -= MenuButtonRight;
        InputController.OnBothButtonsPressedEvent -= MenuButtonBoth;
    }

    private void MenuButtonLeft(InputController controller)
    {
        if (CurrentState == MenuState.Main) ShowCreditsScreen();
        else ShowTitleScreen();
    }

    private void ShowTitleScreen()
    {
        CurrentState = MenuState.NoInput;
        titleScreenUI.SetActive(true);
        StartCoroutine(Tweens.InterpolateRealTime(
            null,
            (t) =>
            {
                // float newY = Tweens.EaseOutBack(targetY - 150f, targetY, t);
                // upgradeBoxL.anchoredPosition = new Vector2(upgradeBoxL.anchoredPosition.x, newY);
                // upgradeBoxR.anchoredPosition = new Vector2(upgradeBoxR.anchoredPosition.x, newY);
            },
            () => { CurrentState = MenuState.Main; },
            0.5f
        ));
        creditsUI.SetActive(false);
    }

    private void ShowCreditsScreen()
    {
        CurrentState = MenuState.Credits;
        creditsUI.SetActive(true);
        StartCoroutine(Tweens.InterpolateRealTime(
            null,
            (t) =>
            {
                // float newY = Tweens.EaseInBack(targetY, targetY - 150f, t);
                // upgradeBoxL.anchoredPosition = new Vector2(upgradeBoxL.anchoredPosition.x, newY);
                // upgradeBoxR.anchoredPosition = new Vector2(upgradeBoxR.anchoredPosition.x, newY);
            },
            () => { CurrentState = MenuState.Credits; },
            0.5f
        ));
        titleScreenUI.SetActive(false);
    }

    private void MenuButtonRight(InputController controller)
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void MenuButtonBoth(InputController controller)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
