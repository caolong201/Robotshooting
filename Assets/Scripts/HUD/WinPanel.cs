using TMPro;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEnemiesKilled;

    void Start()
    {
        txtEnemiesKilled.text = GameManager.Instance.TotalEnemiesKilled.ToString();
    }
    public void OnbtnWinContinueClicked()
    {
        int currentStage = GameManager.Instance.CurrenStage;
        int unlockedStage = PlayerPrefs.GetInt("kUnlockStage", 1);
        if (unlockedStage <= currentStage)
        {
            PlayerPrefs.SetInt("kUnlockStage", currentStage); 
            PlayerPrefs.SetInt("kJustUnlockedNewStage", 1);
            PlayerPrefs.Save();
        }
        ScreenFader.Instance.LoadScene(0); 
    }
}