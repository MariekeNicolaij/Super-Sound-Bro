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
    }
    
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GameOver();
    }

    public void ChangeLifeTime(bool plugIn)
    {
        mainSystem.startLifetime = (plugIn) ? plugInLifeTime : plugOutLifeTime;
        //if (plugIn)
        //    StartCoroutine(RemoveExistingParticles(2));
    }

    IEnumerator RemoveExistingParticles(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        soundSystem.Clear();
    }
}
