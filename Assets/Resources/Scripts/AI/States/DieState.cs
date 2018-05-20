using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IState
{
    float dieAnimationTime = 1.5f;

    // Start
    public void Enter(BaseAI owner)
    {
        owner.dieParticleSystem.Play();
        owner.Invoke("StartDieAnimation", dieAnimationTime);
    }

    // Update
    public void Execute(BaseAI owner)
    {

    }

    public void Exit(BaseAI owner)
    {

    }
}
