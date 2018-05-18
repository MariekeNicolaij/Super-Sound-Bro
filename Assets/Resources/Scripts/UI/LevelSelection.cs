using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Text title;
    public Button playButton;
    public Image previousLevelImage, nextLevelImage, currentLevelImage;
    public Image previousLockImage, nextLockImage, currentLockImage;
    List<Sprite> levelSprites = new List<Sprite>();
    int levelCount;
    int currentLevel = 0;
    int latestUnlockedLevel = 0;


    void Start()
    {
        levelSprites = GetLevelSprites();
        levelCount = GetLevelCount();
        latestUnlockedLevel = GetLatestUnlockedLevel();
        SetLevelImages();
        LockImagesCheck();
    }

    /// <summary>
    /// Dont forget to set them as sprite/UI in editor!
    /// </summary>
    /// <returns></returns>
    List<Sprite> GetLevelSprites()
    {
        List<Sprite> temp = new List<Sprite>();
        foreach (Sprite s in Resources.LoadAll("Sprites/Level Selection", typeof(Sprite)))
            temp.Add(s);
        return temp;
    }

    /// <summary>
    /// Get the amount of levels
    /// </summary>
    /// <returns></returns>
    int GetLevelCount()
    {
        return SceneManager.sceneCountInBuildSettings - 3; // 3 = start, loading and level selection scene
    }

    /// <summary>
    /// Gets the latest unlocked level
    /// </summary>
    /// <returns></returns>
    int GetLatestUnlockedLevel()
    {
        return PlayerPrefs.GetInt("LatestUnlockedLevel", 0);
    }

    /// <summary>
    /// Sets the images, at start the previous gets the last images from list, next gets the second and current gets the first
    /// </summary>
    void SetLevelImages()
    {
        title.text = "Level " + currentLevel;
        previousLevelImage.sprite = levelSprites[IndexCheck(currentLevel - 1)];
        nextLevelImage.sprite = levelSprites[IndexCheck(currentLevel + 1)];
        currentLevelImage.sprite = levelSprites[IndexCheck(currentLevel)];
    }

    void LockImagesCheck()
    {
        // If current level  bigger is than latest unlocked level, lock selection
        bool previousLock = (IndexCheck(currentLevel - 1) > latestUnlockedLevel);
        bool nextLock = (IndexCheck(currentLevel + 1) > latestUnlockedLevel);
        bool currentLock = (IndexCheck(currentLevel) > latestUnlockedLevel);

        playButton.interactable = !currentLock;

        previousLockImage.enabled = previousLock;
        nextLockImage.enabled = nextLock;
        currentLockImage.enabled = currentLock;
    }

    public void Previous()
    {
        currentLevel = IndexCheck(currentLevel - 1);
        SetLevelImages();
        LockImagesCheck();
    }

    public void Next()
    {
        currentLevel = IndexCheck(currentLevel + 1);
        SetLevelImages();
        LockImagesCheck();
    }

    int IndexCheck(int i)
    {
        if (i > levelCount - 1) // -1 bc index you know
            i = 0;
        else if (i < 0)
            i = levelCount - 1; // *

        return i;
    }

    public void StartGame()
    {
        Cursor.visible = false;
        PlayerPrefs.SetString("Scene", "Level " + currentLevel);
        SceneManager.LoadScene("Loading");
    }

    public void Back()
    {
        SceneManager.LoadScene("Start");
    }
}