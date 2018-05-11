using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    BaseAI owner;               // AI

    public IState currentState; // State


    /// <summary>
    /// Sets owner and enters the state
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="startState"></param>
    public StateManager(BaseAI owner, IState startState)
    {
        this.owner = owner;
        currentState = startState;
        currentState.Enter(owner);
    }

    /// <summary>
    /// Exits current state, set new state, Start() new state
    /// </summary>
    /// <param name="newState"></param>
    public void SwitchState(IState newState)
    {
        currentState.Exit(owner);
        currentState = newState;
        currentState.Enter(owner);
    }

    /// <summary>
    /// Update
    /// </summary>
    public void Execute()
    {
        currentState.Execute(owner);
    }
}