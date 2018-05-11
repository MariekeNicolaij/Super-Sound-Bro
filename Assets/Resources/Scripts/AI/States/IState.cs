using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base state for all the states
/// </summary>
public interface IState
{
    void Enter(BaseAI owner);       // Start
    void Execute(BaseAI owner);     // Update
    void Exit(BaseAI owner);        // Exit
}
