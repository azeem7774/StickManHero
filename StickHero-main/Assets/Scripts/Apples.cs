using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apples : MonoBehaviour
{
    [SerializeField] private GameObject m_Partcle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.UpdateApple();
            SoundManager.Instance.PlayPickupSound();
            Instantiate(m_Partcle, transform.position, Quaternion.identity);
            StartCoroutine(Destroy());
            gameObject.SetActive(false);
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        
    }
}
