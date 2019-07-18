using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    GameObject mainMenu;
    GameObject settings;
    GameObject credits;

    void Awake()
    {
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().PlayMusic("Main Theme");
    }
    
    void Start()
    {
        mainMenu = GameObject.Find("Main Screen");
        settings = GameObject.Find("Settings");
        credits = GameObject.Find("Credits");
        settings.SetActive(false);
        credits.SetActive(false);
    }
    
    void Update()
    {
        
    }

    public void PlayGame()
    {
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Main Theme");
        SceneManager.LoadScene(2);
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }

    public void BackToMenu()
    {
        if(settings.activeSelf == true)
        {
            settings.SetActive(false);
        }
        else
        {
            credits.SetActive(false);
        }

        mainMenu.SetActive(true);
    }
}
