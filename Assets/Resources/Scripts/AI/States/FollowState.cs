using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : IState
{
    // Start
    public void Enter(BaseAI owner)
    {
        
    }

    // Update
    public void Execute(BaseAI owner)
    {

    }

    public void Exit(BaseAI owner)
    {
        owner.rBody.velocity = Vector2.zero; // So it doesnt slide
    }
}
