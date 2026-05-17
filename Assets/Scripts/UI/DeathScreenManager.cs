using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI deathMessageText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private Image skullImage;
    [SerializeField] private List<TMPro.TextMeshProUGUI> labels;
    [SerializeField] private GameObject inputs;

    void OnEnable()
    {
        StartCoroutine(DeathScreenAnimationCoroutine());
    }

    private IEnumerator DeathScreenAnimationCoroutine()
    {
        skullImage.color = new Color(1, 1, 1, 0);
        deathMessageText.color = new Color(1, 0, 0, 0);
        scoreText.color = new Color(1, 1, 1, 0);
        foreach (var label in labels) label.color = new Color(1, 1, 1, 0);
        inputs.SetActive(false);

        StartCoroutine(Tweens.InterpolateRealTime(
            null,
            (t) =>
            {
                float newAlpha = Tweens.EaseInOutCubic(0f, 1f, t);
                skullImage.color = new Color(1, 1, 1, newAlpha);
            },
            null,
            2.0f
        ));

        yield return new WaitForSecondsRealtime(2.0f);

        StartCoroutine(Tweens.InterpolateRealTime(
            null,
            (t) =>
            {
                float newAlpha = Tweens.EaseInOutCubic(0f, 1f, t);
                deathMessageText.color = new Color(1, 0, 0, newAlpha);
            },
            null,
            1.5f
        ));

        yield return new WaitForSecondsRealtime(1.0f);

        StartCoroutine(Tweens.InterpolateRealTime(
            null,
            (t) =>
            {
                float newAlpha = Tweens.EaseInOutCubic(0f, 1f, t);
                scoreText.color = new Color(1, 1, 1, newAlpha);
            },
            null,
            1.5f
        ));

        yield return new WaitForSecondsRealtime(1.0f);

        StartCoroutine(Tweens.InterpolateRealTime(
            null,
            (t) =>
            {
                float newAlpha = Tweens.EaseInOutCubic(0f, 1f, t);
                foreach (var label in labels) label.color = new Color(1, 1, 1, newAlpha);
            },
            () =>
            {
                GameManager.Instance.SetGameState(GameManager.GameState.DeathScreen);
                inputs.SetActive(true);
            },
            1.5f
        ));
    }
}
