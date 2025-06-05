
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailedPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEnemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        txtEnemiesKilled.text = SaveDataManager.Instance.TotalEnemiesDeadPerStage.ToString();
    }

    public void OnbtnRetryClicked()
    {
        ScreenFader.Instance.FadeIn(() =>
        {
            SaveDataManager.Instance.LoadScene(EGameState.Lose, () =>
            {
                ScreenFader.Instance.FadeOut();
            });
            
        });
    }
}