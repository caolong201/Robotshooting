
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailedPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEnemiesKilled;
    [SerializeField] private RectTransform retryButton;


    // Start is called before the first frame update
    void Start()
    {
        txtEnemiesKilled.text = GameManager.Instance.TotalEnemiesKilled.ToString();

        if (retryButton != null)
        {
            retryButton.localScale = Vector3.one;

            retryButton.DOScale(1.5f, 0.2f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }

    public void OnbtnRetryClicked()
    {
        ScreenFader.Instance.FadeIn(() =>
        {
            GameManager.Instance.ResetWaves();
            GameManager.Instance.LoadStage();
            ScreenFader.Instance.FadeOut(() =>
            {
                GameManager.Instance.CurrentGameStatus = EGameStatus.Live;
            });
        });
    }
}