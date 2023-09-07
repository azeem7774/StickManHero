using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //making it public static
    public static UIManager _instance;

    [Header("All public variables")]
    public bool isGamestarted = false;


    [Header("All UI Panels")]
    public GameObject[] gamePanels;


    // 0 - MainMenuPanel
    // 1 - gameplay Panel
    // 2 - shop panel

    [Header("Audio Data")]
    public Button soundbtn;
    public GameObject SoundOffImg;
    public bool isSoundEnable = true;

    [Header("Animted Components")]
    public GameObject ShopPanel;
    public GameObject ShopTitle;


    public PlayerController currentPlayer;
    



    #region Unity Call backs


    private void Awake()
    {
        if(_instance==null)
        {
            _instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //Assigning data
        currentPlayer = GameManager.instance.Player.GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #endregion


    #region Custom functions
    public void startGame()
    {
        isGamestarted = true;
        gamePanels[0].SetActive(false);
        gamePanels[1].SetActive(true);
        checkSound();
    }

    public void InvertPlayer()
    {
        if (isGamestarted && GameManager.instance.isMoving)
        {
            Debug.Log("Taped");
            currentPlayer.ChangeDirection();
           
        }
    }

    public void enableShopPanel()
    {
        clearAllPanels();
        gamePanels[2].SetActive(true);
        ShopPanel.GetComponent<Jun_TweenRuntime>().Play();
        ShopTitle.GetComponent<Jun_TweenRuntime>().Play();

    }

    public void enableMainMenu()
    {
        clearAllPanels();
        gamePanels[0].SetActive(true);
    }

   public void enableDisableMusic()
   {

        if (isSoundEnable)
        {
            SoundOffImg.SetActive(true);
            isSoundEnable = false;
            PlayerPrefManager.Sound = 0;
        }
        else
        {
            PlayerPrefManager.Sound = 1;

            SoundOffImg.SetActive(false);
            isSoundEnable = true;

        }

        AudioManager.instance.turnSoundOn(isSoundEnable);

   }

    void checkSound()
    {
        AudioManager.instance.CheckSound();
        if(PlayerPrefManager.Sound==1)
        {
            isSoundEnable = true;
            SoundOffImg.SetActive(false);
        }
        else
        {
            isSoundEnable = true;
            SoundOffImg.SetActive(false);
        }
        AudioManager.instance.turnSoundOn(isSoundEnable);

    }




    //All private functions
    void clearAllPanels()
    {
        for(int i=0;i<gamePanels.Length;i++)
        {
            gamePanels[i].SetActive(false);
        }
    }


    #endregion


}
