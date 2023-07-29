using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayScreen : MonoBehaviour
{
    [SerializeField] private Button m_PlayButton;
    [SerializeField] private GameObject m_LoadingScreen;
    private void Start()
    {
        m_PlayButton.onClick.AddListener(OnClickPlay);
    }

    private void OnClickPlay()
    {
        m_LoadingScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
