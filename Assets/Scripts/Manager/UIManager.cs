using System;
using DG.Tweening;
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

    [SerializeField] private GameObject objTextFindEnemies;

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
}