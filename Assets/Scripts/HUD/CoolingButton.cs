using System.Collections;
using Synty.Interface.MilitaryCombatHUD.Samples;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class CoolingButton : MonoBehaviour
{
    [Header("References")] public Animator myAnimator;
    public TMP_Text text;
    [SerializeField] SampleLoopAnimator fillAnimator;

    [Header("Parameters")] public float initialDelay = 0;
    public float countdownTime = 30;
    public float updateInterval = 0.1f;
    public string timerFormat = "F1";
    public UnityEvent onCountdownComplete;
    [SerializeField] private GameObject tut;
    private float currentTime;

    private void Reset()
    {
        text = GetComponentInChildren<TMP_Text>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    public void Show(bool isShowing)
    {
        gameObject.SetActive(isShowing);
        myAnimator.enabled = false;
        currentTime = countdownTime;
        RefreshUI();
        tut.SetActive(true);
    }

    public void BeginTimer(float delay)
    {
        tut.SetActive(false);
        currentTime = countdownTime;
        RefreshUI();

        myAnimator.enabled = true;
        StartCoroutine(C_TickDown(delay));
        fillAnimator.StartAnimation();
    }

    private IEnumerator C_TickDown(float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        if (myAnimator != null)
        {
            myAnimator.SetBool("Active", true);
            myAnimator.Play("Active"); // sync up the animation
        }

        while (currentTime > 0)
        {
            yield return new WaitForSeconds(updateInterval);

            currentTime -= updateInterval;
            if (currentTime <= 0)
            {
                currentTime = 0;
            }

            RefreshUI();
        }

        myAnimator.enabled = false;
        onCountdownComplete?.Invoke();

        if (myAnimator != null)
        {
            myAnimator.SetBool("Active", false);
        }

        // if (otherObjectAnimator != null)
        // {
        //     if (setOtherObjectAnimatorActive)
        //     {
        //         otherObjectAnimator.gameObject.SetActive(true);
        //     }
        //
        //     otherObjectAnimator.SetBool("Active", true);
        // }
        //
        // yield return new WaitForSeconds(timeUpDuration);
        //
        // if (otherObjectAnimator != null)
        // {
        //     otherObjectAnimator.SetBool("Active", false);
        //     if (setOtherObjectAnimatorActive)
        //     {
        //         otherObjectAnimator.gameObject.SetActive(false);
        //     }
        // }
        //
        // BeginTimer(0);
    }

    private void RefreshUI()
    {
        if (text)
        {
            text.SetText(currentTime.ToString(timerFormat));
        }
    }
}