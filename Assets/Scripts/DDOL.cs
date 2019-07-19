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
    }
}
