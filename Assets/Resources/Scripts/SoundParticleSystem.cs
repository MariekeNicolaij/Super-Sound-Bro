using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundParticleSystem : MonoBehaviour
{
    ParticleSystem soundSystem;
    ParticleSystem.MainModule mainSystem;

    float plugInLifeTime = 1;
    float plugOutLifeTime = 6;

    void Start()
    {
        soundSystem = GetComponent<ParticleSystem>();
        mainSystem = soundSystem.main;
    }
    
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GameOver();
    }

    public void ChangeLifeTime(bool plugIn)
    {
        mainSystem.startLifetime = (plugIn) ? plugInLifeTime : plugOutLifeTime;
    }
}
