using TMPro;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEnemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        txtEnemiesKilled.text = GameManager.Instance.TotalEnemiesKilled.ToString();
        //
        // int unlockStage = PlayerPrefs.GetInt("kUnlockStage", 0);
        // if (SaveDataManager.Instance.Stage > unlockStage && SaveDataManager.Instance.Stage <= SaveDataManager.MAXStage)
        //     PlayerPrefs.SetInt("kUnlockStage", SaveDataManager.Instance.Stage);
    }

    public void OnbtnWinContinueClicked()
    {
        ScreenFader.Instance.FadeIn(() =>
        {
           GameManager.Instance.LoadStage();
           ScreenFader.Instance.FadeOut();
        });
    }
}