using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public bool pathing;

    public PatrolState(Agent _owner, StateManager _stateManager) : base(_owner, _stateManager)
    {

    }

    public override void Enter()
    {
        Debug.Log("Entering Patrol State");
        owner.GetPathing(GameManager.instance.GetRandomGuardLocation());
    }

    public override bool Execute()
    {
        if (owner.HasPath()) owner.MoveCharacterAlongPath();
        else
        {
            owner.GetPathing(GameManager.instance.GetRandomGuardLocation());
            pathing = true;
        }

        return true;
    }

    public override void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }
}
