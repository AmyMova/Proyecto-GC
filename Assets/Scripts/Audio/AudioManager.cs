using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    enum PlayTypes
    {
        MUSIC,
        SFX
    }

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    Sound[] musicSounds;

    [SerializeField]
    AudioSource musicSource;

    [SerializeField]
    Sound[] sfxSounds;

    [SerializeField]
    AudioSource sfxSource;



    private void Start()
    {
        PlayMusic("Theme");
    }

    void PlaySound(Sound[] sounds, AudioSource source, string name, PlayTypes playType)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound != null)
        {
            if (playType == PlayTypes.MUSIC)
            {
                source.clip = sound.sound;
                source.Play();
            }
            else
            {
                source.PlayOneShot(sound.sound);
            }
        }

    }

    public void PlayMusic(string name)
    {
        PlaySound(musicSounds, musicSource, name, PlayTypes.MUSIC);

    }

    public void PlaySFX(string name)
    {
        PlaySound(sfxSounds, sfxSource, name, PlayTypes.SFX);
    }

    public AudioSource GetMusicSource()
    {
        return musicSource;
    }

    public void PauseTheme()
    {
        musicSource.Pause();
    }

    public void ResumeTheme()
    {
        musicSource.UnPause();
    }

}
