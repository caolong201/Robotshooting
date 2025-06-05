using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private void Start()
    {
        if (!SaveDataManager.Instance.IsTutorial)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveDataManager.Instance.IsTutorial && Input.GetMouseButtonDown(0))
        {
            SaveDataManager.Instance.IsTutorial = false;
            gameObject.SetActive(false);
        }
    }
}