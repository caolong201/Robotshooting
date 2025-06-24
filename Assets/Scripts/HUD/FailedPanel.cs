
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailedPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEnemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        txtEnemiesKilled.text = GameManager.Instance.TotalEnemiesKilled.ToString();
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