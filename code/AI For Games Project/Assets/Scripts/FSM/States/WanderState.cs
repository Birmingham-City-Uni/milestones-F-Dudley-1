using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : State
{
    public WanderState(Agent _owner, StateManager _stateManager) : base(_owner, _stateManager)
    {

    }

    public override void Enter()
    {
        Debug.Log("Entering Wander State");
    }

    public override void Execute()
    {
        Debug.Log("Executing Wander State");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Wander State");
    }
}
