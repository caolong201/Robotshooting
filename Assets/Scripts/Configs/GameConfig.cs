using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class StageConfig
{
    public int stage;
    public List<WaveConfig> waves;

    public int TotalWaves()
    {
        return waves.Count;
    }
}

[Serializable]
public class WaveConfig
{
    public int wave;
    public List<EnemyConfig> enemies;
}

[Serializable]
public class EnemyConfig
{
    public float health;
    public float speed;
}

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game Configuration", order = 1)]
public class GameConfig : ScriptableObject
{
    public List<StageConfig> stages;
}

