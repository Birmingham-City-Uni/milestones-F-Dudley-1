using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    /// <summary>
    /// The Patrol States Constructor.
    /// </summary>
    /// <param name="_owner">The States Owner.</param>
    /// <param name="_stateManager">The StateManager the Current State Belongs To.</param>
    /// <returns></returns>
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
            Vector3 newPatrolLocation = VillageManager.instance.GetRandomGuardLocation();
            
            owner.GetPathing(newPatrolLocation);
        }

        return true;
    }

    public override void Exit()
    {
        
    }
}
