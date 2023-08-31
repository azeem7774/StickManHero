using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }



    #endregion


}
