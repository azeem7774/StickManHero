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
        if (SoundManager.Instance.CheckSound())
            m_SoundToggle.isOn = true;
        else
            m_SoundToggle.isOn = false;
        if (SoundManager.Instance.CheckMusic())
            m_MusicToggle.isOn = true;
        else
            m_MusicToggle.isOn = false;

        
        m_CrossButton.onClick.AddListener(OnClickCross);
        m_SoundToggle.onValueChanged.AddListener(ToggleSound);
        m_MusicToggle.onValueChanged.AddListener(ToggleMusic);
        
    }

    private void ToggleMusic(bool isOn)
    {
        if (isOn)
        {
            SoundManager.Instance.TurnOnMusic();
        }
        else
        {
            SoundManager.Instance.TurnOffMusic();
        }
    }

    private void ToggleSound(bool isOn)
    {
        if (isOn)
        {
            SoundManager.Instance.TurnOnSounds();
        }
        else
        {
            SoundManager.Instance.TurnOffSounds();
        }
    }

    private void OnClickCross()
    {
        gameObject.SetActive(false);
    }

    
}
