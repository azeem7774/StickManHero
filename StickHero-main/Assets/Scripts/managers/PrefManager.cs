using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefManager 
{
    private static string PLAYERINDEX = "PLAYERINDEX";

    public static int PlayerIndex
    {
        get
        {
            return PlayerPrefs.GetInt(PLAYERINDEX, 0);
        }

        set
        {
            PlayerPrefs.SetInt(PLAYERINDEX, value);
        }
    }
}
