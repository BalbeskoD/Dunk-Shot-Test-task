using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContinueButton : MonoBehaviour
{
    private PausePanel _pausePanel;
    private Button button;

    private void Awake()
    {
        _pausePanel = GetComponentInParent<PausePanel>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ClosePanel);
    }

    private void ClosePanel()
    {
        _pausePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
