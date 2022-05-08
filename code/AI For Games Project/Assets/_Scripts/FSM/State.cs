using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    /// <summary>
    /// The States Owner.
    /// </summary>
    protected Agent owner;

    /// <summary>
    /// The StateManager The State Belongs To.
    /// </summary>
    protected StateManager stateManager;

    /// <summary>
    /// The State Constructor.
    /// </summary>
    /// <param name="owner">The States Owner.</param>
    /// <param name="stateManager">The StateManager the Current State Belongs To.</param>
    public State(Agent owner, StateManager stateManager)
    {
        this.owner = owner;
        this.stateManager = stateManager;
    }

    /// <summary>
    /// The Function That Runs On Entering of the State.
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// The Function That Runs When Executing The State.
    /// </summary>
    /// <returns>True if Running Correctly, False if State is Finished.</returns>
    public abstract bool Execute();

    /// <summary>
    /// The Function That Runs on Completion of The State.
    /// </summary>
    public abstract void Exit();
}
