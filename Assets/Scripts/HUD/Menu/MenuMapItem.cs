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
    }
    [SerializeField] TextMeshProUGUI textStage;
    [SerializeField] GameObject objCleared, objSelected, objNormal, objLock;
    [SerializeField] Image ObjNormalImage, Normal, Cleared;
    [SerializeField] GameObject locka;
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
    public void PlayUnlockFadeTween()
    {
        objSelected.SetActive(true);
        objNormal.SetActive(true);
        objLock.SetActive(true);

        Image lockImage = objLock.GetComponent<Image>();
        lockImage.color = new Color(1, 1, 1, 1);
        lockImage.DOFade(0, 1f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => locka.SetActive(false));

        RectTransform normalRT = Normal.GetComponent<RectTransform>();
        normalRT.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(normalRT.DOScale(Vector3.one * 1.2f, 1f) // Bung mạnh
                .SetEase(Ease.OutBack));
        seq.Append(normalRT.DORotate(new Vector3(0, 0, 15f), 0.1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(4, LoopType.Yoyo));

        seq.Append(normalRT.DOScale(Vector3.one, 0.6f) 
                      .SetEase(Ease.InOutSine));

        var normalImage = ObjNormalImage;
        normalImage.color = Color.white;
        normalImage.DOColor(Color.green, 0.3f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

    }
    public void PlayClearFadeTween()
    {
        objCleared.SetActive(true);
        objNormal.SetActive(false);
        RectTransform clearedRT = Cleared.GetComponent<RectTransform>();
        clearedRT.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(clearedRT.DOScale(Vector3.one * 1.2f,1f)
            .SetEase(Ease.OutBack));
        seq.Append(clearedRT.DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.InOutSine));
    }

}