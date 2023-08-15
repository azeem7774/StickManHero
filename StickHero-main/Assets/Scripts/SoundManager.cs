using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource m_AudioSourceMusic, m_AudioSourceForPickups;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (SoundPref.Music == 0)
            TurnOffMusic();
        else
            TurnOnMusic();
        if (SoundPref.Sound == 0)
            TurnOffSounds();
        else
            TurnOnSounds();
    }
    public void PlayPickupSound()
    {
        m_AudioSourceForPickups.Play();
    }
    public void TurnOffMusic()
    {
        SoundPref.Music = 0;
        m_AudioSourceMusic.mute = true;
    }

    public void TurnOnMusic()
    {
        SoundPref.Music = 1;
        m_AudioSourceMusic.mute = false;
        m_AudioSourceMusic.Play();
    }

    public void TurnOffSounds()
    {
        SoundPref.Sound = 0;
        m_AudioSourceForPickups.mute = true;
    }

    public void TurnOnSounds()
    {
        SoundPref.Sound = 1;
        m_AudioSourceForPickups.mute = false;
    }

    public  bool CheckSound()
    {
        if (SoundPref.Sound == 0)
            return false;
        else
            return true;
    }

    public bool CheckMusic()
    {
        if (SoundPref.Music == 0)
            return false;
        else
            return true;
    }
}
public static class SoundPref
{
    private static string MUSIC = "MUSIC";
    private static string SOUND = "SOUND";

    public static int Music
    {
        get
        {
            return PlayerPrefs.GetInt(MUSIC);
        }

        set
        {
            PlayerPrefs.SetInt(MUSIC, value);
        }
    }

    public static int Sound
    {
        get
        {
            return PlayerPrefs.GetInt(SOUND);
        }

        set
        {
            PlayerPrefs.SetInt(SOUND, value);
        }
    }
}