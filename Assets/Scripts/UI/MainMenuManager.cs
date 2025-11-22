using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public enum MenuState
    {
        Main,
        Credits
    }

    [SerializeField] private GameObject titleScreenUI;
    [SerializeField] private GameObject creditsUI;
    private MenuState CurrentState = MenuState.Main;

    private void Start()
    {
        CurrentState = MenuState.Main;
        titleScreenUI.SetActive(true);
        creditsUI.SetActive(false);
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
        CurrentState = CurrentState == MenuState.Main ? MenuState.Credits : MenuState.Main;
        titleScreenUI.SetActive(CurrentState == MenuState.Main);
        creditsUI.SetActive(CurrentState == MenuState.Credits);
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
