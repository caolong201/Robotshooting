using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearWave : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveClearText;

    private void OnEnable()
    {
        int currentWave = GameManager.Instance.CurrentWave;
        if (waveClearText != null)
        {
            waveClearText.text = $"Clear Wave {currentWave}";
        }
    }
}
