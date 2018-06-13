using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text title;
    public GameObject startPanel, optionsPanel, audioPanel, quitPanel, levelCompletePanel, pausePanel, gameOverPanel;
    public Slider volumeSlider;
    public Button resetButton;

    public int nonLevelSceneCount = 4;     // Start, Level Selection, Loading, Finished


    void Awake()
    {
        instance = this;

        if (volumeSlider)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeSliderChange);
            volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1) * 100;
            volumeSlider.GetComponentInChildren<Text>().text = PlayerPrefs.GetFloat("MusicVolume", 1) * 100 + "%";
        }

        if (startPanel)
            AnimatePanel(startPanel);
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
        {
            // Focuses on one button so if you use controller you can mage through UI
            EventSystem.current.SetSelectedGameObject(levelCompletePanel.GetComponentInChildren<Button>().gameObject);
            title.text = "Level Complete!";
            AnimatePanel(levelCompletePanel);
        }

    }

    public void TogglePausePanel(bool show)
    {
        Cursor.visible = show;
        pausePanel.SetActive(show);
        if (show)
        {
            // Focuses on one button so if you use controller you can mage through UI
            EventSystem.current.SetSelectedGameObject(pausePanel.GetComponentInChildren<Button>().gameObject);
            title.text = "Pause";
            //AnimatePanel(pausePanel);
        }
    }

    public void ToggleGameOverPanel(bool show)
    {
        Cursor.visible = show;
        gameOverPanel.SetActive(show);
        if (show)
        {
            // Focuses on one button so if you use controller you can mage through UI
            EventSystem.current.SetSelectedGameObject(gameOverPanel.GetComponentInChildren<Button>().gameObject);
            title.text = "Game Over!";
            AnimatePanel(gameOverPanel);
        }
    }

    public void NextLevel()
    {
        AudioManager.instance.ResetMuffleFrequency();
        Time.timeScale = 1;

        PlayerPrefs.SetInt("LatestUnlockedLevel", GetLatestUnlockedLevelIndex());
        SceneManager.LoadScene("Loading");
        string nextLevelName = GetNextLevelName();
        PlayerPrefs.SetString("Scene", nextLevelName);
        if (nextLevelName != "Finished")
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

        bool completedAllLevels = (nextLevelIndex > (SceneManager.sceneCountInBuildSettings - nonLevelSceneCount) - 1); // -1 because index, level 0 also exists

        return (completedAllLevels) ? "Finished" : "Level " + nextLevelIndex;
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
        AnalyticsEvent.Custom("Restarts level", new Dictionary<string, object>
        {
            { "Level", (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - UIManager.instance.nonLevelSceneCount) },
            { "Time", Time.timeSinceLevelLoad }
        });

        AudioManager.instance.ResetMuffleFrequency();
        Time.timeScale = 1;
        SceneManager.LoadScene("Loading");
        PlayerPrefs.SetString("Scene", SceneManager.GetActiveScene().name);
        Cursor.visible = false;
    }

    public void Reset()
    {
        int temp = PlayerPrefs.GetInt("PlayerID");
        PlayerPrefs.DeleteAll();
        resetButton.interactable = false;
        PlayerPrefs.SetInt("PlayerID", temp);
    }

    public void Menu()
    {
        AudioManager.instance.ResetMuffleFrequency();
        Time.timeScale = 1;
        SceneManager.LoadScene("Loading");
        PlayerPrefs.SetString("Scene", "Start");
    }

    public void AnimatePanel(GameObject panel)
    {
        float speed = 1.5f;
        Vector3 tempPos = panel.transform.position;
        //Vector3 tempScale = panel.transform.localScale;

        panel.transform.position = new Vector3(Screen.width, panel.transform.position.y);
        //panel.transform.localScale = Vector3.zero;

        iTween.MoveTo(panel, iTween.Hash("position", tempPos, "time", speed, "easetype", "easeoutelastic"));
        //iTween.ScaleTo(panel, iTween.Hash("scale", tempScale, "time", speed));
    }
}
