using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    public float DisableTime;

    private void OnEnable()
    {
        Invoke("DisableObject", DisableTime);
    }

    void DisableObject()
    {
        this.gameObject.SetActive(false);
    }

}
