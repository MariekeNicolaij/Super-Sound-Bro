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

    bool nextSongTimer = false;
    float nextSongTime = 0;
    [Range(0, 5)]
    public float nextSongDelay = 1.5f;


    void Awake()
    {
        DontDestroyOnLoad(this);

        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        SetMusicList();
        SetVolume();


        PlayRandomMusic();
        Debug.Log("Play random music");
    }

    void Update()
    {
        if (nextSongTimer)
            NextSongTimer();
    }

    void SetMusicList()
    {
        musicList = Resources.LoadAll<AudioClip>("Audio/Music");
    }

    void SetVolume()
    {
        Debug.Log(PlayerPrefs.GetFloat("MusicVolume"));
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

        nextSongTime = song.length + nextSongDelay;
        nextSongTimer = true;

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
        if (musicList.Length > 1)
            while (song == oldSong)
                song = musicList[UnityEngine.Random.Range(0, musicList.Length)];
        else
            song = musicList[UnityEngine.Random.Range(0, musicList.Length)];

        nextSongTime = song.length + nextSongDelay;
        nextSongTimer = true;

        musicSource.clip = song;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    void NextSongTimer()
    {
        if (nextSongTime > 0)
            nextSongTime -= Time.deltaTime;
        else
            PlayRandomMusic();
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