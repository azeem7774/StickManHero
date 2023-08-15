using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image m_LoadingForground;
    [SerializeField] private float m_FackLoadingTime;
    [SerializeField] private GameObject m_CharacterSelection;
    public Action OnLoadingComplete;

    private void OnEnable()
    {
        m_LoadingForground.fillAmount = 0;
        StartCoroutine(FillLoadingBar());
    }

    private IEnumerator FillLoadingBar()
    {
        float elapsedTime = 0;

        while (elapsedTime < m_FackLoadingTime)
        {
            m_LoadingForground.fillAmount = Mathf.Lerp(0, 0.99f, elapsedTime / m_FackLoadingTime);
            elapsedTime += Time.deltaTime;
            yield return null;

        }
        m_LoadingForground.fillAmount = 1;
        gameObject.SetActive(false);
        m_CharacterSelection.SetActive(true);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
