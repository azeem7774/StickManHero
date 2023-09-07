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
        if(Input.GetKeyDown(KeyCode.H))
        {
            ChangeDirection();
        }
    }


    #region My Custom Functions
    public void ChangeDirection()
    {
        this.transform.Rotate(new Vector3(180f, 0f, 0f));
    }



    #endregion





}
