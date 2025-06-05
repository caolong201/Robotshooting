using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameState
{
    Start,
    Win,
    Lose,
    ClearWave
}

public class SaveDataManager : SingletonMonoAwake<SaveDataManager>
{
    Dictionary<int, int> mappingSceneIndexStage = new Dictionary<int, int>()
    {
        { 1, 1 },
        { 2, 2 },
        { 3, 2 },
        { 4, 3 },
        { 5, 3 },
        { 6, 4 },
        { 7, 4 },
        { 8, 5 },
        { 9, 5 },
        { 10, 5 },
    };

    private const int totalScene = 9;
    [SerializeField] GameConfig gameConfig;
    public const int MAXStage = 5;
    public int Stage = 1;
    public int Wave = 1;
    public int TotalEnemiesDeadPerStage = 0;
    public int CountEnemiesPerStage = 0;
    public int CountEnemiesPerWave = 0;

    public bool IsTutorial = true;

    [SerializeField] private StageConfig currentStageConfig;
    public StageConfig CurrentStageConfig => currentStageConfig;

    public override void OnAwake()
    {
        currentStageConfig = GetCurrentStageConfig();
        GetTotalEnemiesPerWaveOrStage();
    }

    // public override void OnStart()
    // {
    //     base.OnStart();
    //    
    // }

    public StageConfig GetCurrentStageConfig()
    {
        return GetStageConfig(Stage);
    }

    public WaveConfig GetCurrentWaveConfig()
    {
        if (currentStageConfig != null)
        {
            return currentStageConfig.waves.Find(x => x.wave == Wave);
        }

        return null;
    }

    public StageConfig GetStageConfig(int stage)
    {
        if (gameConfig != null && gameConfig.stages.Count > 0)
        {
            currentStageConfig = gameConfig.stages.Find(x => x.stage == stage);
            return currentStageConfig;
        }

        return null;
    }

    public void GetTotalEnemiesPerWaveOrStage()
    {
        if (currentStageConfig != null)
        {
            CountEnemiesPerStage = 0;
            CountEnemiesPerWave = 0;
            foreach (var wave in currentStageConfig.waves)
            {
                CountEnemiesPerStage += wave.enemies.Count;
                if (wave.wave == Wave)
                {
                    CountEnemiesPerWave = wave.enemies.Count;
                }
            }
        }
    }

    public void LoadScene(EGameState gameState, System.Action complete = null)
    {
        int indexScene = SceneManager.GetActiveScene().buildIndex;
        switch (gameState)
        {
            case EGameState.Start:
            {
                indexScene = mappingSceneIndexStage.FirstOrDefault(x => x.Value == Stage).Key;
                Wave = 1;
                TotalEnemiesDeadPerStage = 0;
            }
                break;
            case EGameState.Win:
            {
                indexScene++;
                Debug.LogError(indexScene + " is won");
                if (indexScene > totalScene)
                {
                    Stage = Random.Range(1, gameConfig.stages.Count + 1); //randome stage
                    indexScene = mappingSceneIndexStage.FirstOrDefault(x => x.Value == Stage).Key;
                    Debug.LogError(Stage + " $ " + indexScene);
                }
                else
                {
                    Stage++;
                }

                Wave = 1;
                TotalEnemiesDeadPerStage = 0;
            }
                break;
            case EGameState.Lose:
            {
                indexScene = mappingSceneIndexStage.FirstOrDefault(x => x.Value == Stage).Key;
                Wave = 1;
                TotalEnemiesDeadPerStage = 0;
            }
                break;
            case EGameState.ClearWave:
            {
                indexScene++;
                Wave++;
            }
                break;
        }

        currentStageConfig = GetCurrentStageConfig();
        GetTotalEnemiesPerWaveOrStage();

        Debug.Log("LoadScene index: " + indexScene);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(indexScene);
        asyncLoad.completed += (AsyncOperation op) =>
        {
            Debug.Log("Scene " + indexScene + " loaded successfully!");
            DOVirtual.DelayedCall(1f,
                () => { complete?.Invoke(); });
        };
    }

    public void SaveMenuData(List<MenuMapItem.Data> datas)
    {
        string json = JsonUtility.ToJson(datas);
        PlayerPrefs.SetString("kMenuData", json); // Save JSON string
        PlayerPrefs.Save();
        Debug.Log("save: " + json);
    }

    public List<MenuMapItem.Data> LoadMenuData()
    {
        if (PlayerPrefs.HasKey("kMenuData"))
        {
            string json = PlayerPrefs.GetString("kMenuData");
            return JsonUtility.FromJson<List<MenuMapItem.Data>>(json);
        }

        return new List<MenuMapItem.Data>();
    }
}