using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text title;
    public GameObject startPanel, optionsPanel, audioPanel, quitPanel, levelCompletePanel, pausePanel, gameOverPanel;
    public Slider volumeSlider;


    void Awake()
    {
        instance = this;
        if (!FindObjectOfType<AudioManager>())
            gameObject.AddComponent<AudioManager>();

        if (volumeSlider)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeSliderChange);
            volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1) * 100;
            volumeSlider.GetComponentInChildren<Text>().text = PlayerPrefs.GetFloat("MusicVolume", 1) * 100 + "%";
        }
    }

    void OnVolumeSliderChange(float value)
    {
        float finalV = value / 100;

        volumeSlider.GetComponentInChildren<Text>().text = value + "%";

        PlayerPrefs.SetFloat("MusicVolume", finalV);
        AudioManager.instance.musicSource.volume = finalV;
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySound(SoundType.ButtonClick);
    }

    public void Play()
    {
        SceneManager.LoadScene("Loading");
        PlayerPrefs.SetString("Scene", "Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleStartPanel(bool show)
    {
        startPanel.SetActive(show);
        if (show)
            title.text = "Super Sound Bro";
    }

    public void ToggleOptionsPanel(bool show)
    {
        optionsPanel.SetActive(show);
        if (show)
            title.text = "Options";
    }

    public void ToggleAudioPanel(bool show)
    {
        audioPanel.SetActive(show);
        if (show)
            title.text = "Volume";
    }

    public void ToggleQuitPanel(bool show)
    {
        quitPanel.SetActive(show);
        if (show)
            title.text = "Are you sure?";
    }

    public void ToggleLevelCompletePanel(bool show)
    {
        levelCompletePanel.SetActive(show);
        if (show)
            title.text = "Level Complete!";
    }

    public void TogglePausePanel(bool show)
    {
        pausePanel.SetActive(show);
        if (show)
            title.text = "Pause";
    }

    public void ToggleGameOverPanel(bool show)
    {
        gameOverPanel.SetActive(show);
        if (show)
            title.text = (show) ? "Game Over!" : "";
    }

    public void NextLevel()
    {
        Time.timeScale = 1;

        Debug.Log("TBC");
        //SceneManager.LoadScene("Loading");
        //PlayerPrefs.SetString("Scene", SceneManager.GetActiveScene().name);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Loading");
        PlayerPrefs.SetString("Scene", SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Loading");
        PlayerPrefs.SetString("Scene", "Start");
    }
}
