using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEnemies;
    [SerializeField] private Animator animTextEnemies;
    [SerializeField] CoolingButton coolingButton;
    private float coolingTime = 0f;

    void Start()
    {
        OnOverheat(false);
        DOVirtual.DelayedCall(.1f, () =>
        {
            PlayerAutoFire.Instance.onOverheat += OnOverheat;
            
            txtEnemies.text = (SaveDataManager.Instance.CountEnemiesPerWave) + "/" + SaveDataManager.Instance.CountEnemiesPerWave;
            GameManager.Instance.onEnemiesDead += enemiesDead =>
            {
                animTextEnemies.gameObject.SetActive(false);
                animTextEnemies.gameObject.SetActive(true);
                txtEnemies.text = (SaveDataManager.Instance.CountEnemiesPerWave - enemiesDead) + "/" + SaveDataManager.Instance.CountEnemiesPerWave;
            };
        });
    }

    private void OnOverheat(bool isOverheat)
    {
        Debug.Log("OnOverheat: " + isOverheat);
        coolingButton.Show(isOverheat);
    }

    public void OnbtnCoolDownClicked()
    {
        coolingTime = 3f;
        coolingButton.BeginTimer(0);
    }

    private void Update()
    {
        if (coolingTime > 0)
        {
            coolingTime -= Time.deltaTime;
            if (coolingTime <= 0)
            {
                coolingTime = 0;
                PlayerAutoFire.Instance.CoolingDownCompleted();
            }
        }
    }

    private void OnDestroy()
    {
        if (PlayerAutoFire.IsInstanceValid()) PlayerAutoFire.Instance.onOverheat -= OnOverheat;
    }
}