using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundParticleSystem : MonoBehaviour
{
    [HideInInspector]
    public ParticleSystem soundSystem;
    ParticleSystem.MainModule mainSystem;

    float plugInLifeTime = 0;
    float plugOutLifeTime = 6f;

    void Start()
    {
        soundSystem = GetComponent<ParticleSystem>();
        mainSystem = soundSystem.main;
        mainSystem.startLifetime = plugOutLifeTime;
        tag = "Sps";
    }

    public void ChangeLifeTime(bool plugIn)
    {
        mainSystem.startLifetime = (plugIn) ? plugInLifeTime : plugOutLifeTime;
    }
}
