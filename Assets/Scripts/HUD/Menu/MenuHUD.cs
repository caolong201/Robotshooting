using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        // init UI
        for (int i = 0; i < menuItems.Count; i++)                   
        {
            menuItems[i].Init(this, i + 1);
        }
        OnSelectedStage(currentStageSelected);   
        ScrollToStage(currentStageSelected);
        StartPulse();
        UpdateMapLines1();
    }
    private void ScrollToStage(int stage)
    {
        if (stage < 1 || stage > menuItems.Count) return;

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
        PlayerPrefs.SetInt("kCurrentStage", currentStageSelected);
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
    private void UpdateMapLines1()
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
}








