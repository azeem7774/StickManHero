using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefManager 
{

    private static string SOUND = "SOUND";


    public static int Sound
    {
        get
        {
            return PlayerPrefs.GetInt(SOUND);
        }

        set
        {
            PlayerPrefs.SetInt(SOUND, value);
        }
    }


}
