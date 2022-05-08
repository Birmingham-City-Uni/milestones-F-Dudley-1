using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiredState : State
{
    /// <summary>
    /// Holds a Value if The House is Entered.
    /// </summary>
    private bool enteredHouse = false;

    /// <summary>
    /// The TiredStates Constructor.
    /// </summary>
    /// <param name="_owner">The States Owner.</param>
    /// <param name="_stateManager">The StateManager the Current State Belongs To.</param>
    /// <returns></returns>
    public TiredState(Agent _owner, StateManager _stateManager) : base(_owner, _stateManager)
    {

    }

    public override void Enter()
    {
        owner.GetPathing(owner.info.house.GetEntrance());
    }

    public override bool Execute()
    {
        if (owner.info.Tiredness > 30) return false;

        float ownerDistance = owner.DistanceToTarget(owner.info.house.GetEntrance());
        if (ownerDistance <= 0.5f)
        {
            enteredHouse = true;
            owner.info.house.EnterHouse(owner);
        }
        else if (ownerDistance <= 2f)
        {
            owner.MoveCharacterTowardsPoint(owner.info.house.GetEntrance());
        }

        return true;
    }

    public override void Exit()
    {
        Debug.Log("Agent No Longer Tired");
    }
}
