using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_GameScreens;
    private void Start()
    {
        for (int i = 0; i < m_GameScreens.Length; i++) m_GameScreens[i].SetActive(false);
        m_GameScreens[0].SetActive(true);
    }
}
