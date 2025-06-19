using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnimiesManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    private List<EnemyAI> enemies;

    private void Start()
    {
        enemies = new List<EnemyAI>(GetComponentsInChildren<EnemyAI>());
        foreach (var e in enemies)
        {
            e.Init(player);
        }
    }
}