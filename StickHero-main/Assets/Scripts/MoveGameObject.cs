using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGameObject : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxWidth;
    [SerializeField]private RectTransform rect;
    float x;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        x = rect.localPosition.x;
    }
    void Update()
    {
        

        if (rect.localPosition.x >= maxWidth)
        
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        
        else
            rect.localPosition = new Vector3(x,rect.localPosition.y, rect.localPosition.z) ;
    }
}
