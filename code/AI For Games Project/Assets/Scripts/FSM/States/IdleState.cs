using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(Agent _owner, StateManager _stateManager) : base(_owner, _stateManager)
    {

    }

    public override void Enter()
    {
        Debug.Log("Entering Idle State");
    }

    public override bool Execute()
    {
        Debug.Log("Executing Idle State");

        return true;
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
