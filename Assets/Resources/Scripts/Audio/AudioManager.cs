using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // Music
    public AudioClip gameMusic;
    public AudioClip menuMusic;

    // Sfx
    public AudioClip buttonClickSound;
    public AudioClip pickupSound;

    public AudioClip[] musicList;
    AudioClip oldSong;
    [HideInInspector]
    public AudioClip song;

    public float sfxVolume = 100;
    public float musicVolume = 100;


    void Awake()
    {
        instance = this;

        SetMusicList();
        SetVolume();
    }

    void SetMusicList()
    {
        musicList = Resources.LoadAll<AudioClip>("Audio/Music");
    }

    void SetVolume()
    {
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
    }

    public void NextSong()
    {
        musicSource.Stop();
        PlayRandomMusic();
    }

    public void PlaySound(SoundType soundType)
    {
        AudioClip clip = new AudioClip();
        switch (soundType)
        {
            case SoundType.ButtonClick:
                clip = buttonClickSound;
                break;
            case SoundType.Pickup:
                clip = pickupSound;
                break;
        }

        sfxSource.clip = clip;
        sfxSource.volume = sfxVolume;
        sfxSource.Play();
    }

    public void PlayMusic(MusicType musicType)
    {
        if (musicList.Length == 0)
            return;
        musicSource.Stop();

        switch (musicType)
        {
            case MusicType.Menu:
                song = menuMusic;
                break;
            case MusicType.Game:
                song = gameMusic;
                break;
        }

        musicSource.clip = song;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayRandomMusic()
    {
        if (musicList.Length == 0)
            return;

        musicSource.Stop();

        oldSong = song;
        while (song == oldSong)
            song = musicList[UnityEngine.Random.Range(0, musicList.Length)];

        musicSource.clip = song;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }
}

public enum MusicType
{
    Menu,
    Game
}

public enum SoundType
{
    ButtonClick,
    Pickup
}