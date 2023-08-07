using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamplayScreen : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button m_PauseButton;
    [Header("UI Screens")]
    [SerializeField] private GameObject m_PauseScreen;

    private void Start()
    {
        m_PauseButton.onClick.AddListener(OnClickPause);
    }

    private void OnClickPause()
    {
        Time.timeScale = 0;
        m_PauseScreen.SetActive(true);
    }
}
