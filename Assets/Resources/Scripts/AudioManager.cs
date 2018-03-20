using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<GameObject> soundList;

    void Awake()
    {
        instance = this;
        CollectAllSounds();
    }

    void CollectAllSounds()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Sound"))
            soundList.Add(go);
    }

    void Update()
    {

    }
}
