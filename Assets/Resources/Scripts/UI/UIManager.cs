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
    public Button resetButton;

    int nonLevelSceneCount = 3;


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

    /// <summary>
    /// Goes to level selection
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene("Level Selection");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleStartPanel(bool show)
    {
        Cursor.visible = true;
        startPanel.SetActive(show);
        if (show)
            title.text = "Super Sound Bro";
    }

    public void ToggleOptionsPanel(bool show)
    {
        Cursor.visible = true;
        optionsPanel.SetActive(show);
        if (show)
            title.text = "Options";
    }

    public void ToggleAudioPanel(bool show)
    {
        Cursor.visible = true;
        audioPanel.SetActive(show);
        if (show)
            title.text = "Volume";
    }

    public void ToggleQuitPanel(bool show)
    {
        Cursor.visible = true;
        quitPanel.SetActive(show);
        if (show)
            title.text = "Are you sure?";
    }

    public void ToggleLevelCompletePanel(bool show)
    {
        Cursor.visible = show;
        levelCompletePanel.SetActive(show);
        if (show)
            title.text = "Level Complete!";
    }

    public void TogglePausePanel(bool show)
    {
        Cursor.visible = show;
        pausePanel.SetActive(show);
        if (show)
            title.text = "Pause";
    }

    public void ToggleGameOverPanel(bool show)
    {
        Cursor.visible = show;
        gameOverPanel.SetActive(show);
        if (show)
            title.text = (show) ? "Game Over!" : "";
    }

    public void NextLevel()
    {
        Time.timeScale = 1;

        PlayerPrefs.SetInt("LatestUnlockedLevel", GetLatestUnlockedLevelIndex());
        SceneManager.LoadScene("Loading");
        PlayerPrefs.SetString("Scene", GetNextLevelName());
        Cursor.visible = false;
    }

    /// <summary>
    /// Gets next level if exists
    /// Also make sure that the levels are set in the right order buildsettings
    /// </summary>
    /// <returns></returns>
    string GetNextLevelName()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex - nonLevelSceneCount;
        int nextLevelIndex = currentLevelIndex + 1;

        return (nextLevelIndex > SceneManager.sceneCountInBuildSettings) ? "Start" : "Level " + nextLevelIndex;
    }

    /// Gives the latest unlocked level
    /// Checks if you play an old level, it doesnt get set back to that level
    int GetLatestUnlockedLevelIndex()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex - nonLevelSceneCount;
        int nextLevelIndex = currentLevelIndex + 1;
        int latestUnlockedLevel = PlayerPrefs.GetInt("LatestUnlockedLevel", 0);

        if (nextLevelIndex > SceneManager.sceneCountInBuildSettings)
            nextLevelIndex = currentLevelIndex;

        return (nextLevelIndex > latestUnlockedLevel) ? nextLevelIndex : latestUnlockedLevel;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Loading");
        PlayerPrefs.SetString("Scene", SceneManager.GetActiveScene().name);
        Cursor.visible = false;
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        resetButton.interactable = false;
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Loading");
        PlayerPrefs.SetString("Scene", "Start");
    }
}
