using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Stage
{
}

public class GameManager : SingletonMono<GameManager>
{
    public Action<int> onEnemiesDead;
    private int TotalEnemiesDeadPerWave = 0;

    public const int MAXStage = 5;
    public int CurrenStage = 1;
    public int CurrentWave = 1;

    private StageController currStageController = null;
    [SerializeField] Transform playerTransform;

    private int _countEnemiesDeadPerWave = 0;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        LoadStage();
    }

    public void LoadStage()
    {
        UIManager.Instance.Reset();
        if (currStageController != null)
        {
            Destroy(currStageController.gameObject);
            currStageController = null;
        }

        GameObject stagePrefab = Resources.Load<GameObject>("Stages/" + CurrenStage);
        if (stagePrefab != null)
        {
            // Instantiate it in the scene
            GameObject stageInstance = Instantiate(stagePrefab);

            // Get the StageController component if needed
            currStageController = stageInstance.GetComponent<StageController>();

            if (currStageController != null)
            {
                // Now you can use the controller
                currStageController.Init(playerTransform, CurrentWave); // Or any setup logic you have
            }
            else
            {
                Debug.LogWarning("StageController component not found on prefab.");
            }
        }
    }

    public void EnemyDead()
    {
        _countEnemiesDeadPerWave++;
        Debug.Log("EnemyDead: " + _countEnemiesDeadPerWave);
        int totalEnemiesPerWave = currStageController.GetWave().GetEnemiesCount();
        if (_countEnemiesDeadPerWave >= totalEnemiesPerWave)
        {
            if (CurrentWave >= currStageController.WaveCount())
            {
                //next stage
                _countEnemiesDeadPerWave = 0;
                CurrentWave = 1;
                CurrenStage++;
                // LoadStage();
                DOVirtual.DelayedCall(1.5f, () => { UIManager.Instance.ShowEndGame(true); });
            }
            else
            {
                Debug.Log("Game Clear wave");
                DOVirtual.DelayedCall(1f, () =>
                {
                    UIManager.Instance.ShowWaveClear(() =>
                    {
                        _countEnemiesDeadPerWave = 0;
                        CurrentWave++;
                        currStageController.Init(playerTransform, CurrentWave);
                    });
                });
            }
        }

        return;
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
                    DOVirtual.DelayedCall(1f,
                        () =>
                        {
                            UIManager.Instance.ShowWaveClear(() =>
                            {
                                SaveDataManager.Instance.LoadScene(EGameState.ClearWave,
                                    () => { ScreenFader.Instance.FadeOut(); });
                            });
                        });
                }
            }
        }

        onEnemiesDead?.Invoke(TotalEnemiesDeadPerWave);
    }
}