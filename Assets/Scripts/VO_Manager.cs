using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VO_Manager : MonoBehaviour
{
    public static VO_Manager instance;
    public AudioFile[] voiceFiles;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if(!PlayerPrefs.HasKey("VO Volume"))
        {
            PlayerPrefs.SetFloat("VO Volume", 0.7f);
        }

        foreach (AudioFile file in voiceFiles)
        {
            file.source = gameObject.AddComponent<AudioSource>();
            file.source.clip = file.audioClip;
            file.source.volume = file.volume;
            file.source.loop = file.isLooping;

            //if (file.playOnAwake)
            //{
            //    if (PlayerPrefs.HasKey("BGM Volume"))
            //    {
            //        file.source.volume = PlayerPrefs.GetFloat("BGM Volume");
            //        file.source.Play();
            //    }
            //    else
            //    {
            //        file.source.Play();
            //    }
            //}
        }
    }

    void Start()
    {

    }

    public void PlayVoice(string name)
    {
        AudioFile file = Array.Find(instance.voiceFiles, AudioFile => AudioFile.audioName == name);
        file.source.Play();
    }

    public void StopVoice(string name)
    {
        AudioFile file = Array.Find(instance.voiceFiles, AudioFile => AudioFile.audioName == name);
        file.source.Stop();
    }

    public void PauseVoice(string name)
    {
        AudioFile file = Array.Find(instance.voiceFiles, AudioFile => AudioFile.audioName == name);
        file.source.Pause();
    }

    public void UnPauseVoice(string name)
    {
        AudioFile file = Array.Find(instance.voiceFiles, AudioFile => AudioFile.audioName == name);
        file.source.UnPause();
    }
}
