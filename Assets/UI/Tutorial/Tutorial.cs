
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance.CurrenStage == 1 && GameManager.Instance.CurrentWave == 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }
}