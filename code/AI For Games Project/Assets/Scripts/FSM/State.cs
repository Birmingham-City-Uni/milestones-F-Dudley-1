using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Agent owner;
    protected StateManager stateManager;

    public State(Agent owner, StateManager stateManager)
    {
        this.owner = owner;
        this.stateManager = stateManager;
    }

    public abstract void Enter();
    public abstract bool Execute();
    public abstract void Exit();
}
