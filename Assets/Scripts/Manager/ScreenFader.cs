using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ScreenFader : SingletonMonoAwake<ScreenFader>
{
    public CanvasGroup FadeImg;

    public override void OnAwake()
    {
        base.OnAwake();
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

    public void LoadScene(int indexScene, System.Action complete = null)
    {
        FadeIn(() =>
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(indexScene);
            asyncLoad.completed += (AsyncOperation op) =>
            {
                Debug.Log("Scene " + indexScene + " loaded successfully!");
                FadeOut(() =>
                {
                    complete?.Invoke();
                });
            };
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