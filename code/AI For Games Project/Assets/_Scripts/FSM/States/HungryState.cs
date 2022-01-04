using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryState : State
{
    /// <summary>
    /// The Constructor Of The Hungry State.
    /// </summary>
    /// <param name="_owner">The States Owner.</param>
    /// <param name="_stateManager">The StateManager the Current State Belongs To.</param>
    /// <returns></returns>
    public HungryState(Agent _owner, StateManager _stateManager) : base(_owner, _stateManager)
    {

    }

    public override void Enter()
    {
        
    }

    public override bool Execute()
    {
        return true;
    }

    public override void Exit()
    {
        
    }
}
