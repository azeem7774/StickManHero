using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private Character[] m_Players;
    [SerializeField] private GameObject[] m_Characters;
    [SerializeField] private Button m_Next, m_Previous, m_Select, m_Buy;
    [SerializeField] private GameObject m_LoadingScreen;
    [SerializeField] private TextMeshProUGUI m_Oranges, m_PlayerPrice, m_NotEnoughCash;
    [SerializeField] private LoadingScreen m_Loading;
    private string PlayerUnlock = "PlayerUnlock";

    private int m_PlayerIndex;
    private void OnEnable()
    {
        m_Oranges.text = PlayerPrefs.GetInt("Diamonds").ToString();
        UnlockPlayers();
    }
    private void Start()
    {
        PlayerPrefs.SetInt("Diamonds", 1000);
        m_PlayerIndex = PrefManager.PlayerIndex;
        m_Next.onClick.AddListener(OnClickNext);
        m_Previous.onClick.AddListener(OnClickPrevious);
        m_Select.onClick.AddListener(OnClickSelect);
        m_Buy.onClick.AddListener(OnClickBuy);
        TurnOnPlayer(m_PlayerIndex);
        if (!IsPlayerLocked(m_PlayerIndex))
        {
            m_Buy.gameObject.SetActive(false);
            m_Select.gameObject.SetActive(true);
        }
    }

    private void OnClickBuy()
    {
        int oranges = PlayerPrefs.GetInt("Diamonds");
        if (m_Players[m_PlayerIndex].m_Price < oranges)
        {
            oranges -= m_Players[m_PlayerIndex].m_Price;
            PlayerPrefs.SetInt("Diamonds", oranges);
            m_Oranges.text = oranges.ToString();
            m_Buy.gameObject.SetActive(false);
            m_Select.gameObject.SetActive(true);
            PlayerPrefs.SetInt(PlayerUnlock + m_PlayerIndex, 100); // 100 means player is unlocked
        }
        else
        {

        }
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
        if (IsPlayerLocked(m_PlayerIndex))
        {
            m_Buy.gameObject.SetActive(true);
            m_Select.gameObject.SetActive(false);
            m_PlayerPrice.text = m_Players[m_PlayerIndex].m_Price.ToString();
        }
        else
        {
            m_Buy.gameObject.SetActive(false);
            m_Select.gameObject.SetActive(true);
        }
        
    }

    private void OnClickNext()
    {
        m_PlayerIndex = (m_PlayerIndex + 1) % m_Characters.Length;
        TurnOnPlayer(m_PlayerIndex);
        if (IsPlayerLocked(m_PlayerIndex))
        {
            m_Buy.gameObject.SetActive(true);
            m_Select.gameObject.SetActive(false);
            m_PlayerPrice.text = m_Players[m_PlayerIndex].m_Price.ToString();
        }
        else
        {
            m_Buy.gameObject.SetActive(false);
            m_Select.gameObject.SetActive(true);
        }
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

    void UnlockPlayers()
    {
        for (int i = 1; i < m_Players.Length; i++)
        {
            if (PlayerPrefs.GetInt(PlayerUnlock+i) == 100)
            {
                m_Players[i].m_IsLocked = false;
            }
        }
    }
    private bool IsPlayerLocked(int index)
    {
        if (m_Players[index].m_IsLocked)
        {
            return true;
        }
        return false;
    }
}
[Serializable] public struct Character
{
    public int m_Id;
    public bool m_IsLocked;
    public int m_Price;
    public GameObject m_CharacterPrefab;
}