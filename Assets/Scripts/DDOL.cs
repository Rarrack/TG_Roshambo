using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (!PlayerPrefs.HasKey("Scene"))
        {
            PlayerPrefs.SetInt("Scene", 0);
        }

        if (!PlayerPrefs.HasKey("Total Wins"))
        {
            PlayerPrefs.SetInt("Total Wins", 0);
        }
    }
}
