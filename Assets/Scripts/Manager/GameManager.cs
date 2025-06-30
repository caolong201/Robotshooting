using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EGameStatus
{
    Live,
    End
}

public class GameManager : SingletonMono<GameManager>
{
    private int TotalEnemiesDeadPerWave = 0;

    private const int MAXStage = 10;
    public int CurrenStage = 1;
    public int CurrentWave = 1;

    private StageController currStageController = null;
    [SerializeField] Transform playerTransform;

    private int _countEnemiesDeadPerWave = 0;
    public int TotalEnemiesKilled = 0;
    public EGameStatus CurrentGameStatus = EGameStatus.Live;
    [SerializeField] TransitionWave transitionWave;
    [SerializeField] bool debugStage = false;

    [HideInInspector] public bool IsRayHitEmeny = false;
    [SerializeField] private Tracker trackerPrefab;

    [HideInInspector] public bool IsGunReloading = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
#if !UNITY_EDITOR
        CurrenStage = PlayerPrefs.GetInt("kCurrentStage", 1);
#else
        if (!debugStage)
        {
            CurrenStage = PlayerPrefs.GetInt("kCurrentStage", 1);
        }
#endif

        ResetWaves();
    }

    private void Start()
    {
        LoadStage();
        ScreenFader.Instance.FadeOut();
    }

    public void ResetWaves()
    {
        _countEnemiesDeadPerWave = 0;
        CurrentWave = 1;
    }

    public void LoadStage()
    {
        TotalEnemiesKilled = 0;
        UIManager.Instance.Reset();
        if (currStageController != null)
        {
            Destroy(currStageController.gameObject);
            currStageController = null;
        }

        int stageIndex = CurrenStage;
        if (stageIndex > MAXStage)
        {
            stageIndex = Random.Range(2, MAXStage + 1);
        }

        GameObject stagePrefab = Resources.Load<GameObject>("Stages/" + stageIndex);
        if (stagePrefab != null)
        {
            // Instantiate it in the scene
            GameObject stageInstance = Instantiate(stagePrefab);

            // Get the StageController component if needed
            currStageController = stageInstance.GetComponent<StageController>();

            if (currStageController != null)
            {
                // Now you can use the controller
                currStageController.Init(playerTransform, CurrentWave, trackerPrefab); // Or any setup logic you have

                //rsheald
                playerTransform.GetComponent<PlayerController>().ResetHealth();
                UIManager.Instance.ShowHealthBar(true);
                UIManager.Instance.UpdateStageText(CurrenStage);
            }
            else
            {
                Debug.LogWarning("StageController component not found on prefab.");
            }
        }
    }

    public void EnemyDead()
    {
        TotalEnemiesKilled++;
        _countEnemiesDeadPerWave++;
        Debug.Log("EnemyDead: " + _countEnemiesDeadPerWave);
        int totalEnemiesPerWave = currStageController.GetWave().GetEnemiesCount();
        if (_countEnemiesDeadPerWave >= totalEnemiesPerWave)
        {
           
            if (CurrentWave >= currStageController.WaveCount())
            {
                CurrentGameStatus = EGameStatus.End;
                ResetWaves();
                CurrenStage++;
                PlayerPrefs.SetInt("kCurrentStage", CurrenStage);

                int unlockStage = PlayerPrefs.GetInt("kUnlockStage", 1);
                if (CurrenStage > unlockStage)
                {
                    PlayerPrefs.SetInt("kUnlockStage", CurrenStage);
                }

                DOVirtual.DelayedCall(1.5f, () => { UIManager.Instance.ShowEndGame(true); });
            }

            else
            {
                Debug.Log("Game Clear wave");
                UIManager.Instance.ShowHealthBar(false);
                UIManager.Instance.ShowCrossHair(false);
                
                IsGunReloading = true;
                DOVirtual.DelayedCall(2.5f, () =>
                {
                    CurrentGameStatus = EGameStatus.End;
                    _countEnemiesDeadPerWave = 0;
                    CurrentWave++;

                    currStageController.Init(playerTransform, CurrentWave, trackerPrefab);
                    PlayerPrefs.SetInt("kCurrentWave", CurrentWave);
                  
                    transitionWave.StartTransition(currStageController.GetWave().GetTarget(),
                        () =>
                        {
                            UIManager.Instance.ShowHealthBar(true);
                            UIManager.Instance.ShowCrossHair(true);
                            CurrentGameStatus = EGameStatus.Live;
                        });
                });
            }
        }
    }
}