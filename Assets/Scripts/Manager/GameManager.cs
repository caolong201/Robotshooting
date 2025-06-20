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
    public Action<int> onEnemiesDead;
    private int TotalEnemiesDeadPerWave = 0;

    public const int MAXStage = 5;
    public int CurrenStage = 1;
    public int CurrentWave = 1;
    public bool IsTutorial = true;

    private StageController currStageController = null;
    [SerializeField] Transform playerTransform;

    private int _countEnemiesDeadPerWave = 0;
    public int TotalEnemiesKilled = 0;
    public EGameStatus CurrentGameStatus = EGameStatus.Live;
    [SerializeField] TransitionWave transitionWave;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        IsTutorial = PlayerPrefs.GetInt("kTutorial", 0) == 0 ? true : false;
        //#if  !UNITY_EDITOR
        CurrenStage = PlayerPrefs.GetInt("kCurrentStage", 1);
        CurrentWave = PlayerPrefs.GetInt("kCurrentWave", 1);
//#endif
    }

    private void Start()
    {
        
        LoadStage();
        ScreenFader.Instance.FadeOut();
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
            stageIndex = Random.Range(1, MAXStage + 1);
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
                currStageController.Init(playerTransform, CurrentWave); // Or any setup logic you have

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

        CurrentGameStatus = EGameStatus.Live;
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
                _countEnemiesDeadPerWave = 0;
                CurrentWave = 1;

                CurrenStage++;

             
                PlayerPrefs.SetInt("kCurrentStage", CurrenStage);

            
                int unlockStage = PlayerPrefs.GetInt("kUnlockStage", 1);
                if (CurrenStage > unlockStage)
                {
                    PlayerPrefs.SetInt("kUnlockStage", CurrenStage);
                }

                PlayerPrefs.Save();

                DOVirtual.DelayedCall(1.5f, () => { UIManager.Instance.ShowEndGame(true); });
            }

            else
            {
                Debug.Log("Game Clear wave");
                UIManager.Instance.ShowHealthBar(false);
                UIManager.Instance.ShowCrossHair(false);
                UIManager.Instance.ShowWaveClear(() => { });
                DOVirtual.DelayedCall(1f, () =>
                {
                    _countEnemiesDeadPerWave = 0;
                    CurrentWave++;
                    Vector3 pos = playerTransform.position;
                    Quaternion rot = playerTransform.rotation;
                    currStageController.Init(playerTransform, CurrentWave);
                    PlayerPrefs.SetInt("kCurrentWave", CurrentWave);
    
                    playerTransform.gameObject.SetActive(false);
                    transitionWave.StartTransition(pos,
                        currStageController.GetWave().GetPlayerPosition(),
                        rot,
                        () =>
                        {
                            playerTransform.gameObject.SetActive(true);
                            UIManager.Instance.ShowHealthBar(true);
                            UIManager.Instance.ShowCrossHair(true);
                        });
                    
                });
            }
        }
    }

    public bool IsShowFXLastHit()
    {
        if (CurrenStage == 1 && CurrentWave == 2)
        {
            
        }

        return false;
    }
}