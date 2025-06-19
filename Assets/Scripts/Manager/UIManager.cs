using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : SingletonMono<UIManager>
{
    [SerializeField] Image blackScreen;
    [SerializeField] private GameObject winPanel, losePanel, waveClearPanel;
    [SerializeField] private Image fxHpLow, fxTakeDamage;
    private bool isHpLow = false;
    private bool isTakeDamage = false;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject objTextFindEnemies;
    [SerializeField] private GameObject boxSlider, crosshair;

    [SerializeField] private TextMeshProUGUI stageText;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        waveClearPanel.SetActive(false);
        Close();
    }

    public void ShowEndGame(bool isWin)
    {
        GameManager.Instance.CurrentGameStatus = EGameStatus.End;
        winPanel.SetActive(isWin);
        losePanel.SetActive(!isWin);

        ShowBlackScreen();
        ShowHealthBar(false);
        if (isWin)
        {
            winPanel.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), .2f);
        }
        else
        {
            losePanel.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), .2f);
        }

        if (fxHpLow != null) fxHpLow.gameObject.SetActive(false);
    }

    public void ShowWaveClear(System.Action complete)
    {
        waveClearPanel.SetActive(true);
        ShowBlackScreen();
        DOVirtual.DelayedCall(1f, () =>
        {
            complete?.Invoke();
            waveClearPanel.SetActive(false);
            Close();
        });
        
        if (fxHpLow != null) fxHpLow.gameObject.SetActive(false);
    }

    public void ShowBlackScreen()
    {
        blackScreen.DOFade(0.8f, 0.2f);
        blackScreen.raycastTarget = true;
    }

    public void Close()
    {
        blackScreen.DOFade(0, 0f);
        blackScreen.raycastTarget = false;
    }

    public void OnHPLow()
    {
        if (fxHpLow != null && !isHpLow)
        {
            isHpLow = true;
            fxHpLow.gameObject.SetActive(true);
            fxHpLow.DOKill();
            // Set initial alpha to 0.5
            Color startColor = fxHpLow.color;
            startColor.a = 0.5f;
            fxHpLow.color = startColor;

            // Looping fade effect from 0.5 to 1 and back to 0.5
            fxHpLow.DOFade(1f, 0.5f) // Fade to 1
                .SetLoops(-1, LoopType.Yoyo) // Infinite loop: 1 -> 0.5 -> 1
                .SetEase(Ease.InOutSine); // Smooth fade effect
        }
    }

    public void OnTakeDamage()
    {
        if (fxTakeDamage != null && !isTakeDamage)
        {
            isTakeDamage = true;
            fxTakeDamage.gameObject.SetActive(true);
            fxTakeDamage.DOKill();
            Color startColor = fxTakeDamage.color;
            startColor.a = 0f;
            fxTakeDamage.color = startColor;

            fxTakeDamage.DOFade(1f, 0.15f) // Fade to 1
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => { isTakeDamage = false; });
        }
    }

    public void ShowTextFindEnemies(bool isShow)
    {
        if (objTextFindEnemies)
        {
            objTextFindEnemies.SetActive(isShow);
        }
    }
    public void SetHealthSliderMax(float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{maxHealth}/{maxHealth}";
        }
    }

    public void UpdateHealthSlider(float currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.DOValue(currentHealth, 0.2f);
        }

        if (healthText != null)
        {           
            healthText.text = $"{Mathf.CeilToInt(currentHealth)}";
        }
    }

    public void ShowHealthBar(bool isShow)
    {
        if (boxSlider != null)
            boxSlider.SetActive(isShow);
    }
    
    public void ShowCrossHair(bool isShow)
    {
        if (crosshair != null)
            crosshair.SetActive(isShow);
    }
    public void UpdateStageText(int currentStage)
    {
        if (stageText != null)
        {
            //stageText.text = $"Stage {currentStage}";


            if (stageText != null)
            {
                stageText.text = $"STAGE {currentStage}";
                stageText.transform.DOKill();
                stageText.transform.localScale = Vector3.one;
                stageText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
            }
        }
    }

   
}