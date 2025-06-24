using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject root;
    private bool isShowing = false;
    void Update()
    {
        if (GameManager.Instance.CurrentGameStatus != EGameStatus.Live)
        {
            root.SetActive(false);
            isShowing = false;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (isShowing)
            {
                root.SetActive(false);
                isShowing = false;
            }
        }
        else
        {
            if (!isShowing && !GameManager.Instance.IsRayHitEmeny)
            {
                root.SetActive(true);
                isShowing = true;
            }
        }
    }
}