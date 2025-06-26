using System;
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
    private string timerFormat = "F2";
    private Action onCountdownComplete;
    [SerializeField] private GameObject reloadIcon;
    private float currentTime;

    private bool isShowing = false;

    private void Reset()
    {
        text = GetComponentInChildren<TMP_Text>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    public void Show(bool isShowing)
    {
        if (this.isShowing == isShowing) return;

        this.isShowing = isShowing;
        gameObject.SetActive(isShowing);
        myAnimator.enabled = false;
        currentTime = countdownTime;
        RefreshUI();
        reloadIcon.SetActive(this.isShowing);
    }

    public void BeginTimer(float delay, Action callback)
    {
        onCountdownComplete = callback;
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
    }

    private void RefreshUI()
    {
        if (text)
        {
            text.SetText(currentTime.ToString(timerFormat));
        }
    }
}