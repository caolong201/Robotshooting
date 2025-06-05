using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEnemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        txtEnemiesKilled.text = SaveDataManager.Instance.TotalEnemiesDeadPerStage.ToString();

        Debug.Log("kUnlockStage: " + SaveDataManager.Instance.Stage);
        int unlockStage = PlayerPrefs.GetInt("kUnlockStage", 0);
        if (SaveDataManager.Instance.Stage > unlockStage && SaveDataManager.Instance.Stage <= SaveDataManager.MAXStage)
            PlayerPrefs.SetInt("kUnlockStage", SaveDataManager.Instance.Stage);
    }

    public void OnbtnWinContinueClicked()
    {
        ScreenFader.Instance.FadeIn(() =>
        {
            SaveDataManager.Instance.LoadScene(EGameState.Win, () =>
            {
                ScreenFader.Instance.FadeOut();
            });
           
        });
    }
}