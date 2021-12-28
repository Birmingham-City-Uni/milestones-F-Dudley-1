using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryState : State
{
    public HungryState(Agent _owner, StateManager _stateManager) : base(_owner, _stateManager)
    {

    }

    public override void Enter()
    {
        
    }

    public override bool Execute()
    {
        if (owner.MoveCharacterAlongPath())
        {

        }
        else
        {
            
        }

        return true;
    }

    public override void Exit()
    {
        
    }
}
