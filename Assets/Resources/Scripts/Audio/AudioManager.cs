using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // Muffle
    public AudioLowPassFilter muffleFilter;
    [Range(0, 2000)]
    public float muffleFrequency = 1500;
    float oldMuffleFrequency;

    // Music
    public AudioClip gameMusic;
    public AudioClip menuMusic;

    // Sfx
    public AudioClip buttonClickSound;
    public AudioClip pickupSound;
<<<<<<< HEAD
=======
    public AudioClip deathSound;
<<<<<<< HEAD
    public AudioClip victorySound;
>>>>>>> parent of 59f4302... Revert "Added victory sound"
=======
>>>>>>> parent of 91cea2e... Added victory sound

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

        oldMuffleFrequency = muffleFilter.cutoffFrequency;
    }

    public void ResetMuffleFrequency()
    {
        muffleFilter.cutoffFrequency = oldMuffleFrequency;
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
        AudioClip clip = null;
        switch (soundType)
        {
            case SoundType.ButtonClick:
                clip = buttonClickSound;
                break;
            case SoundType.Pickup:
                clip = pickupSound;
                break;
<<<<<<< HEAD
=======
            case SoundType.Death:
                clip = deathSound;
                break;
<<<<<<< HEAD
            case SoundType.Victory:
                clip = victorySound;
                break;
>>>>>>> parent of 59f4302... Revert "Added victory sound"
=======
>>>>>>> parent of 91cea2e... Added victory sound
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
<<<<<<< HEAD
    Pickup
=======
    Pickup,
<<<<<<< HEAD
    Death,
    Victory
>>>>>>> parent of 59f4302... Revert "Added victory sound"
=======
    Death
>>>>>>> parent of 91cea2e... Added victory sound
}