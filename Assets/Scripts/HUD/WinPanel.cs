using TMPro;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEnemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        txtEnemiesKilled.text = GameManager.Instance.TotalEnemiesKilled.ToString();
    }

    public void OnbtnWinContinueClicked()
    {
        // ScreenFader.Instance.FadeIn(() =>
        // {
        //    GameManager.Instance.LoadStage();
        //    ScreenFader.Instance.FadeOut();
        // });
        ScreenFader.Instance.LoadScene(0);
    }
}