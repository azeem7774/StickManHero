using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    //making it instance
    public static AudioManager instance;


    [Header("Details for the audio Sources")]
    public AudioSource BgSound;
    public AudioClip pickupSound;



    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

    }

    #region Unity Call backs
    // Start is called before the first frame update
    void Start()
    {
        CheckSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion



    #region Custom Callbacks
    public void CheckSound()
    {
        if (PlayerPrefManager.Sound == 1)
        {
            BgSound.Play();
        }
    }

    public void turnSoundOn(bool soundVal)
    {
        if (soundVal)
        {
            BgSound.Play();
        }
        else
        {
            BgSound.Stop();
        }
    }

    #endregion

}
