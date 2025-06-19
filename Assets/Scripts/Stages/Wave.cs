using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private EnemiesManager _enemiesManager;
    private PlayerPos playerPos;
    
    public void Init(Transform player)
    {
        _enemiesManager = GetComponentInChildren<EnemiesManager>();
        _enemiesManager.Init(player);
        
        playerPos = GetComponentInChildren<PlayerPos>();
        
        player.position = playerPos.transform.position;
        player.rotation = playerPos.transform.rotation;
        
        
    }

    public int GetEnemiesCount()
    {
        return _enemiesManager.EnemiesCount();
    }
    
    public Vector3 GetPlayerPosition()
    {
        return playerPos.transform.position;
    }
}
