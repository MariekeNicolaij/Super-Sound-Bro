﻿using UnityEngine;

public class BaseAI : MonoBehaviour
{
    public Player player;                // Target to follow/attack/flee
    [HideInInspector]
    public StateManager stateManager;       // Handles the states

    public ParticleSystem dieParticleSystem;

    [Range(0, 25)]
    public float speed;


    void Start()
    {
        // If target has not been set then target will be the player by default
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (!dieParticleSystem)
            dieParticleSystem = GetComponentInChildren<ParticleSystem>();

        stateManager = new StateManager(this, new IdleState());     // Handles the states
    }

    void Update()
    {
        stateManager.Execute();                          // Update stateManager
    }

    void StartDieAnimation()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Weapon")
            stateManager.SwitchState(new DieState());
    }

    /// <summary>
    /// Either left or right
    /// </summary>
    /// <returns></returns>
    public Vector2 RandomDirection()
    {
        int chanceFactor = Random.Range(0, 100);
        int chance = 50; // %

        return (chanceFactor < chance) ? Vector2.left : Vector2.right;
    }
}