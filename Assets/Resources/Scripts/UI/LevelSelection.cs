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

    // pn = previous and next, c = current
    Vector3 pnScale, cScale;
    float animationSpeed = 0.75f;


    void Start()
    {
        levelSprites = GetLevelSprites();
        levelCount = GetLevelCount();
        latestUnlockedLevel = GetLatestUnlockedLevel();
        SetLevelImages();
        LockImagesCheck();

        pnScale = previousLevelImage.transform.localScale;              // Set standard scale
        cScale = currentLevelImage.transform.localScale;                // Set standard scale

        AnimateImages(false);
    }

    void Update()
    {
        InputCheck();
    }

    /// <summary>
    /// Go through levels with keys instead of buttons
    /// </summary>
    void InputCheck()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button4))
            Previous();
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Joystick1Button5))
            Next();
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
        StartCoroutine(ChangeLevelImages(animationSpeed, false));
    }

    public void Next()
    {
        StartCoroutine(ChangeLevelImages(animationSpeed, true));
    }

    public IEnumerator ChangeLevelImages(float delayInSeconds, bool next)
    {
        // Set current level
        currentLevel = (next) ? IndexCheck(currentLevel + 1) : IndexCheck(currentLevel - 1);

        // Animate the images before they change
        AnimateImages(true);

        // Wait till animation is finished
        yield return new WaitForSeconds(delayInSeconds);

        // Set new images
        SetLevelImages();
        // Check if they are unlocked
        LockImagesCheck();

        // Animate the images back
        AnimateImages(false);
    }

    int IndexCheck(int i)
    {
        if (i > levelCount - 1) // -1 bc index you know
            i = 0;
        else if (i < 0)
            i = levelCount - 1; // *

        return i;
    }

    void AnimateImages(bool fadeOut)
    {
        if (fadeOut)
        {
            // scale level images to zero
            iTween.ScaleTo(previousLevelImage.gameObject, iTween.Hash("scale", Vector3.zero, "time", animationSpeed, "easetype", "easeinoutexpo"));
            iTween.ScaleTo(currentLevelImage.gameObject, iTween.Hash("scale", Vector3.zero, "time", animationSpeed, "easetype", "easeinoutexpo"));
            iTween.ScaleTo(nextLevelImage.gameObject, iTween.Hash("scale", Vector3.zero, "time", animationSpeed, "easetype", "easeinoutexpo"));
        }
        else
        {
            // scale level images to normal size
            iTween.ScaleTo(previousLevelImage.gameObject, iTween.Hash("scale", pnScale, "time", animationSpeed, "easetype", "easeinoutexpo"));
            iTween.ScaleTo(currentLevelImage.gameObject, iTween.Hash("scale", cScale, "time", animationSpeed, "easetype", "easeinoutexpo"));
            iTween.ScaleTo(nextLevelImage.gameObject, iTween.Hash("scale", pnScale, "time", animationSpeed, "easetype", "easeinoutexpo"));
        }
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