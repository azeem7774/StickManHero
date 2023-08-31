using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int jumpCounter=1;
    public float jumpPower;
    public Animator _anim;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb=this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            // Loop through all active touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                // Check if this touch is a tap (e.g., finger released)
                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    // Handle the tap here
                    Debug.Log("Tapped on screen");
                }
            }
        }
    }
    private void OnMouseDown()
    {
        Debug.Log("Tap On Player");


        //if(GameManager.instance.isMoving)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        //    Invoke("disableJump", 0.5f);
        //}
    }

    void disableJump()
    {
        GameManager.instance.isMoving = false;
    }


}
