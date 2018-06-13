using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerID : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerID"))
            PlayerPrefs.SetInt("PlayerID", Random.Range(0, int.MaxValue));
    }
}
