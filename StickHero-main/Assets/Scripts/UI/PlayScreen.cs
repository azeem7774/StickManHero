using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayScreen : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button m_PlayButton;
    [SerializeField] private Button m_SettingButton;
    [Header("UI Screens")]
    [SerializeField] private GameObject m_LoadingScreen;
    [SerializeField] private GameObject m_Settings;
    private void Start()
    {
        m_PlayButton.onClick.AddListener(OnClickPlay);
        m_SettingButton.onClick.AddListener(OnClickSetting);
    }

    private void OnClickSetting()
    {
        m_Settings.SetActive(true);
    }

    private void OnClickPlay()
    {
        m_LoadingScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
