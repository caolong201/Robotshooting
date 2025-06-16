using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnimiesManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    private List<EnemyController> enemies;

    private void Start()
    {
        enemies = new List<EnemyController>(GetComponentsInChildren<EnemyController>(true));
        foreach (var e in enemies)
        {
            e.Init(player);
        }
    }
}