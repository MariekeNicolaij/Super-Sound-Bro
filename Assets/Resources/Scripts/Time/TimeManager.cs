using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance; 
    public List<int> bestTimes;
    public Text timeText;


    void Awake()
    {
        timeText = GetTimeText();
    }

    Text GetTimeText()
    {
        return GameObject.FindGameObjectWithTag("TimeText").GetComponent<Text>();
    }

    void Update()
    {

    }
}
