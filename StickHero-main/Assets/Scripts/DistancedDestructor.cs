using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancedDestructor : MonoBehaviour
{

    private GameObject player;
    public Transform ObjectPoint;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(!player)
        {
            player = GameObject.Find("Player");
            return;
        }
        if(player.transform.position.x - transform.position.x > 15f)
        {
            Destroy(gameObject);
        }

        if(UIManager._instance.isGamestarted)
        {
            this.gameObject.SetActive(true);
        }

    }

    private void OnEnable()
    {
        
    }


}
