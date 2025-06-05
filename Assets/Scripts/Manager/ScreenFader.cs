using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class ScreenFader : SingletonMonoStart<ScreenFader>
{
    public CanvasGroup FadeImg;

    void Awake()
    {
        FadeImg.DOFade(0, 0);
        FadeImg.blocksRaycasts = false;
    }

    public void FadeIn(float duration, System.Action complete = null)
    {
        FadeImg.blocksRaycasts = true;
        FadeImg.DOFade(1, duration).OnComplete(() => { complete?.Invoke(); });
    }
    
    public void FadeIn(System.Action complete = null)
    {
        FadeIn(0.5f, complete);
    }

    public void FadeOut(System.Action complete = null)
    {
        FadeImg.DOFade(0, 0.5f).OnComplete(() =>
        {
            FadeImg.blocksRaycasts = false;
            complete?.Invoke();
        });
    }

    public void FadeInOut(System.Action complete = null)
    {
        FadeImg.blocksRaycasts = true;
        FadeIn(() => { FadeOut(() =>
        {
            FadeImg.blocksRaycasts = false;
            complete?.Invoke();
        }); });
    }
}