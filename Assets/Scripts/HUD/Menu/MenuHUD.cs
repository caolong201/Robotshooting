using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHUD : MonoBehaviour
{
    [SerializeField] List<MenuMapItem> menuItems = new List<MenuMapItem>();
    public RectTransform textTransform;
    public RectTransform buttonTransform;
    private int currentStageSelected = 1;
    public float minScale = 0.8f; 
    public float maxScale = 1.2f; 
    public float pulseDuration = 0.4f; 
    private Tween pulseTween;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] List<Image> mapLines = new List<Image>();
    public Color unlockedLineColor = Color.yellow;
    public Color lockedLineColor = Color.gray;


    void Start()
    {
        currentStageSelected = PlayerPrefs.GetInt("kCurrentStage", 1);
        if (currentStageSelected == 1)
        {
            ScreenFader.Instance.FadeIn(0);
            OnbtnPlayClicked();
            return;
        }
        // Init UI
        for (int i = 0; i < menuItems.Count; i++)
        {
            menuItems[i].Init(this, i + 1);
        }
        if (PlayerPrefs.GetInt("kJustUnlockedNewStage", 0) == 1)
        {
            int unlockStage = PlayerPrefs.GetInt("kUnlockStage", 1);

            if (unlockStage <= menuItems.Count)
            {
                // Gọi hiệu ứng clear cho stage trước đó
                if (unlockStage > 1)
                {
                    menuItems[unlockStage - 2].PlayClearFadeTween();
                }

                menuItems[unlockStage - 1].PlayUnlockFadeTween();
                PlayLineGlowTween(unlockStage);
            }

            PlayerPrefs.SetInt("kJustUnlockedNewStage", 0);
            PlayerPrefs.Save();
        }
        OnSelectedStage(currentStageSelected);   
        ScrollToStage(currentStageSelected);
        StartPulse();
        UpdateMapLines();
    }
    private void ScrollToStage(int stage)
    {

        if (stage < 1) return;
        if (stage > menuItems.Count) 
        {
            stage = 20;
        };

        RectTransform target = menuItems[stage - 1].GetComponent<RectTransform>();
        float targetPosY = Mathf.Abs(target.localPosition.y);
        float contentHeight = scrollRect.content.rect.height - scrollRect.viewport.rect.height;
        float offset = 900f;
        float normalizedPosition = 1 - Mathf.Clamp01((targetPosY - offset) / contentHeight);
        //scrollRect.DOVerticalNormalizedPos(normalizedPosition, 0.5f).SetEase(Ease.OutCubic);
        scrollRect.verticalNormalizedPosition = normalizedPosition;
    }
  
    public void OnSelectedStage(int stage)
    {
        currentStageSelected = stage;
        PlayerPrefs.SetInt("kCurrentStage", currentStageSelected );
        PlayerPrefs.Save();

        currentStageSelected = stage;
        foreach (var item in menuItems)
        {
            if (item.mStage == stage)
            {
                Debug.Log("lv" + stage);
                item.Select(true);
                item.Select(item.mStage == stage);
            }
            else
            {
                item.Select(false);
            }
        }
    }
    public void OnbtnPlayClicked()
    {
        PlayerPrefs.SetInt("kCurrentStage", currentStageSelected);
        PlayerPrefs.SetInt("kCurrentWave", 1);
        ScreenFader.Instance.LoadScene(1);
    }
    public void StartPulse()
    {
        buttonTransform.localScale = Vector3.one;
        pulseTween = buttonTransform
            .DOScale(minScale, pulseDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

    }
    public void StopPulse()
    {
        if (pulseTween != null)
        {
            pulseTween.Kill();
            buttonTransform.localScale = Vector3.one;
        }
    }
    private void UpdateMapLines()
    {
        for (int i = 0; i < mapLines.Count; i++)
        {
            if (mapLines[i] == null) continue; 

            if (i < currentStageSelected - 1)
            {             
                mapLines[i].color = unlockedLineColor;
            }
            else
            { 
                mapLines[i].color = lockedLineColor;
            }
        }
    }


    public void PlayLineGlowTween(int stageIndex)
    {
        int lineIndex = stageIndex - 2;
        if (lineIndex < 0 || lineIndex >= mapLines.Count) return;

        Image line = mapLines[lineIndex];
        line.color = unlockedLineColor;

        line.DOFade(0.2f, 0.4f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                line.color = unlockedLineColor;
            });
    }
}








