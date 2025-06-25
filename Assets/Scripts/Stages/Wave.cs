using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private EnemiesManager _enemiesManager;
    private PlayerPos playerPos;

    public void Init(Transform player, int wave, Tracker tracker)
    {
        _enemiesManager = GetComponentInChildren<EnemiesManager>();
        _enemiesManager.Init(player,tracker);

        playerPos = GetComponentInChildren<PlayerPos>();

        if (wave == 1)
        {
            player.position = playerPos.transform.position;
            player.rotation = playerPos.transform.rotation;
        }
    }

    public int GetEnemiesCount()
    {
        return _enemiesManager.EnemiesCount();
    }

    public Transform GetTarget()
    {
        return playerPos.transform;
    }
}