using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Text title;          // Display title
    AsyncOperation operation;   // Loading
    string scene;               // Which scene to load


    void Start()
    {
        scene = PlayerPrefs.GetString("Scene");
        StartCoroutine(Load()); // This will be called multiple times untill it has finished the operation
    }

    IEnumerator Load()
    {
        operation = SceneManager.LoadSceneAsync(scene); // Scene to load
        operation.allowSceneActivation = false;         // We do not want the scene to activate immediately

        while (!operation.isDone)
        {
            if (operation.progress < 0.9f)  // From 0.9 it is loaded somehow
            {
                title.text = "Loading: " + Mathf.RoundToInt(operation.progress * 100) + "%";        // Shows progress on screen
            }
            else // if progress >= 0.9f the scene is loaded and is ready to activate.
            {
                title.text = "Press any key";

                if (Input.anyKeyDown)
                    operation.allowSceneActivation = true;      // Activates scene when a button is pressed
            }
            yield return null;
        }
    }
}