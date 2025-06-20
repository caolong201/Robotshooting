using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHUD : MonoBehaviour
{
    [SerializeField] List<MenuMapItem> menuItems = new List<MenuMapItem>();

    private int currentStageSelected = 1;

    // Start is called before the first frame update
    void Start()
    {
        // init UI
        for (int i = 0; i < menuItems.Count; i++)
        {
            menuItems[i].Init(this, i + 1);
        }

        //ScreenFader.Instance.FadeIn(0);
    }

    public void OnSelectedStage(int stage)
    {
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
        //SaveDataManager.Instance.Stage = currentStageSelected;
        ScreenFader.Instance.LoadScene(1);
    }
}