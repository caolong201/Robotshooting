using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] private GameObject visibleObject;
    [SerializeField] private Camera mainCamera;
    private EnemyAI currentObjective;
    private bool isActive = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void SetTarget(EnemyAI objective)
    {
        currentObjective = objective;
        visibleObject.SetActive(currentObjective != null);
        this.isActive = true;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameStatus != EGameStatus.Live)
        {
            visibleObject.SetActive(false);
            return;
        }

        if (currentObjective == null) return;
        UpdateTrackerPositionAndRotation();
    }

    private void UpdateTrackerPositionAndRotation()
    {
        if (!this.isActive || mainCamera == null) return;

        if (this.currentObjective.isDead)
        {
            this.isActive = false;
            this.visibleObject.SetActive(false);
            return;
        }

        Vector3 screenPoint = mainCamera.WorldToScreenPoint(currentObjective.transform.position);
        bool isOffScreen = screenPoint.z < 0 ||
                           screenPoint.x < 0 || screenPoint.x > Screen.width ||
                           screenPoint.y < 0 || screenPoint.y > Screen.height;

        visibleObject.SetActive(isOffScreen);

        if (isOffScreen)
        {
            if (screenPoint.z < 0)
            {
                screenPoint.x = -screenPoint.x;
                screenPoint.y = -screenPoint.y;
            }

            float padding = 50f;
            screenPoint.x = Mathf.Clamp(screenPoint.x, padding, Screen.width - padding);
            screenPoint.y = Mathf.Clamp(screenPoint.y, padding, Screen.height - padding);

            Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            Vector3 directionFromCenter = (screenPoint - screenCenter).normalized;
            float angle = Mathf.Atan2(directionFromCenter.y, directionFromCenter.x) * Mathf.Rad2Deg;

            if (screenPoint.z < 0)
            {
                angle += 180f;
            }

            Vector3 trackerPosition = new Vector3(screenPoint.x, screenPoint.y, 0);
           
            transform.position = trackerPosition;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
