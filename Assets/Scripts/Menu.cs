using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    GameObject mainMenu;
    GameObject settings;
    GameObject credits;

    GameObject fadeScreen;
    int testWaitTime = 0;
    bool fadeStart = false;

    void Awake()
    {
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().PlayMusic("Main Theme");
    }
    
    void Start()
    {
        mainMenu = GameObject.Find("Main Screen");
        settings = GameObject.Find("Settings");
        credits = GameObject.Find("Credits");
        fadeScreen = GameObject.Find("Fade Screen");
        settings.SetActive(false);
        credits.SetActive(false);
        fadeScreen.SetActive(false);
    }
    
    void Update()
    {
        if (fadeStart == true)
        {
            testWaitTime += 1;
            if (testWaitTime >= 55)
            {
                SceneManager.LoadScene(2);
            }
        }
    }

    public void PlayGame()
    {
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Main Theme");
        GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Start Game");
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<Animator>().Play("Anim_fade");
        fadeStart = true;
    }

    public void Settings()
    {
        GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Select");
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void Credits()
    {
        GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Select");
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

        GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Back");
        mainMenu.SetActive(true);
    }
}
