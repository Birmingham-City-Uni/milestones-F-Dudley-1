using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{
    public SeekState(Agent _owner, StateManager _stateManager) : base(_owner, _stateManager)
    {

    }

    public override void Enter()
    {
        Debug.Log("Entering Seek State");
    }

    public override bool Execute()
    {
        Debug.Log("Executing Seek State");

        return true;
    }

    public override void Exit()
    {
        Debug.Log("Exiting Seek State");
    }
}
