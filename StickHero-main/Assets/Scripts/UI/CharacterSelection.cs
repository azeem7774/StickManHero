using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] m_Characters;
    [SerializeField] private Button m_Next, m_Previous, m_Select;
    [SerializeField] private GameObject m_LoadingScreen;
    private int m_PlayerIndex;
    private void Start()
    {
        m_PlayerIndex = PrefManager.PlayerIndex;
        m_Next.onClick.AddListener(OnClickNext);
        m_Previous.onClick.AddListener(OnClickPrevious);
        m_Select.onClick.AddListener(OnClickSelect);
        
        TurnOnPlayer(m_PlayerIndex);
    }

    private void OnClickSelect()
    {
        PrefManager.PlayerIndex = m_PlayerIndex;
        m_LoadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync(1);
    }

    private void OnClickPrevious()
    {
        m_PlayerIndex = (m_PlayerIndex + m_Characters.Length - 1) % m_Characters.Length;
        TurnOnPlayer(m_PlayerIndex);
    }

    private void OnClickNext()
    {
        m_PlayerIndex = (m_PlayerIndex + 1) % m_Characters.Length;
        TurnOnPlayer(m_PlayerIndex);
    }

    void TurnOnPlayer(int index)
    {
        TurnOffAllPlayers();
        m_Characters[index].SetActive(true);
    }
    void TurnOffAllPlayers()
    {
        for (int i = 0; i < m_Characters.Length; i++) m_Characters[i].SetActive(false);
    }
}
