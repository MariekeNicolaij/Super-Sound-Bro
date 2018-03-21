using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    public bool isFinishPlug;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (isFinishPlug)
            if (other.tag == "Plug")
                LevelComplete();
    }

    void LevelComplete()
    {
        UIManager.instance.ShowLevelCompletePanel();
        Time.timeScale = 0;
    }
}
