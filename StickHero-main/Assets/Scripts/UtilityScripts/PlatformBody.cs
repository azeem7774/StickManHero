using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBody : MonoBehaviour
{



    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        Debug.LogError(go.tag);
        if(go.CompareTag("Player"))
        {
            go.GetComponent<Rigidbody2D>().gravityScale = 100f;
            GameManager.instance.GameOver();
        }

    }

}
