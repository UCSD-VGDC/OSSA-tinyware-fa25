using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
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

    public EventReference bgmRef;
    public EventReference buttonRef;

    [SerializeField] private EventInstance bgmInstance;

    [SerializeField] private GameObject titleScreenUI;
    [SerializeField] private GameObject creditsUI;
    [SerializeField] private Image blackOverlay;
    [SerializeField] private List<TMPro.TextMeshProUGUI> logoTexts;
    [SerializeField] private List<RectTransform> buttons;
    [SerializeField] private List<RectTransform> labels;
    private MenuState CurrentState = MenuState.NoInput;

    private void Start()
    {
        titleScreenUI.SetActive(true);
        creditsUI.SetActive(false);
        // StartCoroutine(Tweens.Interpolate(
        //     null,
        //     (t) =>
        //     {
        //         float newAlpha = Tweens.EaseInOutCubic(1f, 0f, t);
        //         blackOverlay.color = new Color(0, 0, 0, newAlpha);
        //     },
        //     () => { StartCoroutine(ShowTitleScreenCoroutine()); },
        //     2.5f
        // ));
        bgmInstance = RuntimeManager.CreateInstance(bgmRef);
        bgmInstance.start();
        CurrentState = MenuState.Main;
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
        RuntimeManager.PlayOneShot(buttonRef);
        if (CurrentState == MenuState.Main) ShowCreditsScreen();
        else ShowMainMenuScreen();
    }

    private IEnumerator ShowTitleScreenCoroutine()
    {
        CurrentState = MenuState.NoInput;
        creditsUI.SetActive(false);
        titleScreenUI.SetActive(true);

        StartCoroutine(Tweens.Interpolate(
            null,
            (t) =>
            {
                float newAlpha = Tweens.EaseInOutCubic(0f, 1f, t);
                foreach (var logoText in logoTexts) logoText.color = new Color(0.25f, 0.25f, 0.25f, newAlpha);
            },
            null,
            2.5f
        ));

        yield return new WaitForSecondsRealtime(3f);
        float targetY = -70f;

        StartCoroutine(Tweens.InterpolateRealTime(
            null,
            (t) =>
            {
                float newY = Tweens.EaseOutQuart(targetY - 150f, targetY, t);
                foreach (var button in buttons) button.anchoredPosition = new Vector2(button.anchoredPosition.x, newY);
                foreach (var label in labels) label.anchoredPosition = new Vector2(label.anchoredPosition.x, newY + 32f);
            },
            () => { CurrentState = MenuState.Main; },
            2.5f
        ));

        //bgmInstance.start();
    }

    private void ShowMainMenuScreen()
    {
        titleScreenUI.SetActive(true);
        creditsUI.SetActive(false);
        CurrentState = MenuState.Main;
    }

    private void ShowCreditsScreen()
    {
        titleScreenUI.SetActive(false);
        creditsUI.SetActive(true);
        CurrentState = MenuState.Credits;
    }

    private void MenuButtonRight(InputController controller)
    {
        
        RuntimeManager.PlayOneShot(buttonRef);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void MenuButtonBoth(InputController controller)
    {
        RuntimeManager.PlayOneShot(buttonRef);
        bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
