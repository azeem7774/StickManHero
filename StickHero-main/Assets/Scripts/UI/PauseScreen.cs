using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Button m_Resume, m_Home;
    private void Start()
    {
        m_Resume.onClick.AddListener(OnClickResume);
        m_Home.onClick.AddListener(OnClickHome);
    }

    private void OnClickHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void OnClickResume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
