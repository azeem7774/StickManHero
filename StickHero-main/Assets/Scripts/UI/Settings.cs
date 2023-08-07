using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Toggle m_SoundToggle, m_MusicToggle;
    [SerializeField] private Button m_CrossButton;

    private void Start()
    {
        m_CrossButton.onClick.AddListener(OnClickCross);
    }

    private void OnClickCross()
    {
        gameObject.SetActive(false);
    }
}
