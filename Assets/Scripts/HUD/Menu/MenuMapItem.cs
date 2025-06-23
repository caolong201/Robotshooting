using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuMapItem : MonoBehaviour
{
    public class Data
    {
        public int Stage = 1;
        //        public int clearStage = 0;
    }
    [SerializeField] TextMeshProUGUI textStage;
    [SerializeField] GameObject objCleared, objSelected, objNormal, objLock;
    private MenuHUD parent;
    public int mStage = 1;
    public void Init(MenuHUD parent, int stage)
    {
        mStage = stage;
        this.parent = parent;
        int unlockStage = PlayerPrefs.GetInt("kUnlockStage", 1);
        objCleared.SetActive(false);
        objNormal.SetActive(false);
        objLock.SetActive(false);
        objSelected.SetActive(false);


        textStage.text = stage.ToString();

        if (unlockStage > stage)
        {
            objCleared.SetActive(true);
        }
        else if (unlockStage == stage)
        {
            objSelected.SetActive(true);
            objNormal.SetActive(true);
        }
        else
        {
            objNormal.SetActive(true);
            objLock.SetActive(true);
            GetComponent<Button>().interactable = false;
        }
    }
    public void Select(bool isSelected)
    {
        objSelected.SetActive(isSelected);
    }

    public void OnClick()
    {
        Debug.Log("OnClick: " + mStage);
        parent.OnSelectedStage(mStage);
    }
}