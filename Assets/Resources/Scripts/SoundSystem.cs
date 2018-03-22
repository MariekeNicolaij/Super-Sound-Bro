using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    ParticleSystem soundSystem;
    ParticleSystem.MainModule mainSystem;

    void Start()
    {
        soundSystem = GetComponent<ParticleSystem>();
        mainSystem = soundSystem.main;
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        if (other.transform.tag == "Player")
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GameOver();

    }
}
