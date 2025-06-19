using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private List<EnemyAI> enemies;

    public void Init(Transform player)
    {
        enemies = new List<EnemyAI>(GetComponentsInChildren<EnemyAI>());
        foreach (var e in enemies)
        {
            e.Init(this, player);
        }
    }

    public void EnemyDead(EnemyAI enemy)
    {
        GameManager.Instance.EnemyDead();
    }

    public int EnemiesCount()
    {
        if (enemies == null) return 0;
        return enemies.Count;
    }
}