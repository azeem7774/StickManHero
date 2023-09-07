using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oranges : MonoBehaviour
{
    [SerializeField] private GameObject m_Partcle;
    private GameObject temp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with" + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            GameManager.instance.UpdateOranges();
            //SoundManager.Instance.PlayPickupSound();
            temp = Instantiate(m_Partcle, transform.position, Quaternion.identity);
            StartCoroutine(Destroy());
            gameObject.SetActive(false);
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(temp);
        Destroy(gameObject);

    }
}
