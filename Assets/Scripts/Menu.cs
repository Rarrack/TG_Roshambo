using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    GameObject mainMenu;
    GameObject settings;
    GameObject credits;

    public Slider bgmSlider;
    public Slider sfxSlider;

    public GameObject[] Locks;

    GameObject fadeScreen;
    int testWaitTime = 0;
    bool fadeStart = false;
    public bool multiplayer;

    void Awake()
    {
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().musicFiles[0].source.volume = PlayerPrefs.GetFloat("BGM Volume");
        GameObject.Find("__sfx").GetComponent<SFX_Manager>().soundFiles[0].source.volume = PlayerPrefs.GetFloat("SFX Volume");
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().PlayMusic("Main Theme");
    }
    
    void Start()
    {
        mainMenu = GameObject.Find("Main Screen");
        settings = GameObject.Find("Settings");
        credits = GameObject.Find("Credits");
        fadeScreen = GameObject.Find("Fade Screen");

        bgmSlider.value = PlayerPrefs.GetFloat("BGM Volume", 0.7f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX Volume", 0.7f);

        if (PlayerPrefs.GetInt("Scene") == 0)
        {
            settings.SetActive(false);
            credits.SetActive(false);
            fadeScreen.SetActive(false);
        }
        else if(PlayerPrefs.GetInt("Scene") == 1)
        {
            mainMenu.SetActive(false);
            settings.SetActive(false);
            fadeScreen.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            credits.SetActive(false);
            fadeScreen.SetActive(false);
            if(PlayerPrefs.GetInt("Total Wins") >= 3)
            {
                Locks[0].SetActive(false);
            }
            if (PlayerPrefs.GetInt("Total Wins") >= 5)
            {
                Locks[1].SetActive(false);
            }
        }
    }
    
    void Update()
    {
        if (fadeStart == true)
        {
            testWaitTime += 1;
            if (testWaitTime >= 55)
            {
                if (multiplayer != true)
                {
                    SceneManager.LoadScene(2);
                }
                else
                {
                    SceneManager.LoadScene(3);
                }
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

    public void PlayMultiplayer()
    {
        GameObject.Find("__bgm").GetComponent<BGM_Manager>().StopMusic("Main Theme");
        GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Start Game");
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<Animator>().Play("Anim_fade");
        fadeStart = true;
        multiplayer = true;
    }

    public void Settings()
    {
        GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Select");
        mainMenu.SetActive(false);
        settings.SetActive(true);
        bgmSlider.value = PlayerPrefs.GetFloat("BGM Volume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX Volume");
        if (PlayerPrefs.GetInt("Total Wins") >= 3)
        {
            Locks[0].SetActive(false);
        }
        if (PlayerPrefs.GetInt("Total Wins") >= 5)
        {
            Locks[1].SetActive(false);
        }
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

        PlayerPrefs.SetInt("Scene", 0);
        GameObject.Find("__sfx").GetComponent<SFX_Manager>().PlaySound("Back");
        mainMenu.SetActive(true);
    }

    public void SetBGMVolume(float vol)
    {
        PlayerPrefs.SetFloat("BGM Volume", vol);
    }

    public void SetSFXVolume(float vol)
    {
        PlayerPrefs.SetFloat("SFX Volume", vol);
    }

    public void UpdateBGMVolumes()
    {
        foreach(AudioFile bgm in GameObject.Find("__bgm").GetComponent<BGM_Manager>().musicFiles)
        {
            bgm.source.volume = PlayerPrefs.GetFloat("BGM Volume");
        }
    }

    public void UpdateSFXVolumes()
    {
        foreach (AudioFile sfx in GameObject.Find("__sfx").GetComponent<SFX_Manager>().soundFiles)
        {
            sfx.source.volume = PlayerPrefs.GetFloat("SFX Volume");
        }
    }
}
