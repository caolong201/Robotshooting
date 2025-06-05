using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using VSX.UniversalVehicleCombat.Radar;
using Random = UnityEngine.Random;

public class GameManager : SingletonMono<GameManager>
{
    [SerializeField] GroupTrackerUpdater groupTrackerUpdater;


    public Action<int> onEnemiesDead;
    private int TotalEnemiesDeadPerWave = 0;

    private void Awake()
    {
        Application.targetFrameRate = 60;

    }

    private void Start()
    {
        DOVirtual.DelayedCall(0.5f,
            () =>
            {
                groupTrackerUpdater.SetupEnemiesPerStage(SaveDataManager.Instance.Stage, SaveDataManager.Instance.Wave);
            });
    }

    public void EnemyDead()
    {
        SaveDataManager.Instance.TotalEnemiesDeadPerStage++;
        TotalEnemiesDeadPerWave++;
        
        if (SaveDataManager.Instance != null)
        {
            if (SaveDataManager.Instance.TotalEnemiesDeadPerStage >= SaveDataManager.Instance.CountEnemiesPerStage)
            {
                Debug.Log("Game Win");
                DOVirtual.DelayedCall(1.5f, () => { UIManager.Instance.ShowEndGame(true); });
            }
            else
            {
                if (TotalEnemiesDeadPerWave == SaveDataManager.Instance.CountEnemiesPerWave)
                {
                    Debug.Log("Game Clear wave");
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        UIManager.Instance.ShowWaveClear(() =>
                        {
                            SaveDataManager.Instance.LoadScene(EGameState.ClearWave, () =>
                            {
                                ScreenFader.Instance.FadeOut();
                            });
                        });
                    });
                }
            }
        }

        onEnemiesDead?.Invoke(TotalEnemiesDeadPerWave);
    }
}