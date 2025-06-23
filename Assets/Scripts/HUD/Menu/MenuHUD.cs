using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
        StartPulse();
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
                item.Select(true);
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
}


    


  
       
   

