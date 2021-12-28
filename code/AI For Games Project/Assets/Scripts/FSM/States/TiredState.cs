using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiredState : State
{
    private bool enteredHouse;

    public TiredState(Agent _owner, StateManager _stateManager) : base(_owner, _stateManager)
    {

    }

    public override void Enter()
    {
        owner.GetPathing(owner.info.house.GetEntrance());
    }

    public override bool Execute()
    {
        if (owner.info.tiredness > 30) return false;

        if (owner.MoveCharacterAlongPath())
        {

        }
        else
        {
            enteredHouse = true;
            owner.info.house.EnterHouse(owner);
        }

        return true;
    }

    public override void Exit()
    {
        Debug.Log("Agent No Longer Tired");
    }
}
