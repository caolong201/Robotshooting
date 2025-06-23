using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private List<Wave> waves = null;
    private int currentWave = 0;
    public void Init(Transform player, int currentWave)
    {
        this.currentWave = currentWave;
        waves = new List<Wave>(GetComponentsInChildren<Wave>(true));
        for (int i = 0; i < waves.Count; i++)
        {
            if (currentWave - 1 == i)
            {
                waves[i].gameObject.SetActive(true);
                waves[i].Init(player,currentWave);
            }
            else
            {
                waves[i].gameObject.SetActive(false);
            }
        }
    }

    public int WaveCount()
    {
        if (waves == null) return 0;

        return waves.Count;
    }

    public Wave GetWave()
    {
        if (waves != null && currentWave > 0 && currentWave <= waves.Count)
        {
            return waves[currentWave - 1];
        }

        return null;
    }
    
}