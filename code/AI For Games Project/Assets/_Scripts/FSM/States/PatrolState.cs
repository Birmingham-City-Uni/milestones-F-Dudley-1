using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public PatrolState(Agent _owner, StateManager _stateManager) : base(_owner, _stateManager)
    {

    }

    public override void Enter()
    {
        owner.GetPathing(VillageManager.instance.GetRandomGuardLocation());
    }

    public override bool Execute()
    {
        if (!owner.HasPath())
        {
            owner.GetPathing(VillageManager.instance.GetRandomGuardLocation());
        }

        return true;
    }

    public override void Exit()
    {
        
    }
}
