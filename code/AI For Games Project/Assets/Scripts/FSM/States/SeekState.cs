using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{
    bool waitedInArea;
    Coroutine waitingCoroutine;

    public SeekState(Agent _owner, StateManager _stateManager) : base(_owner, _stateManager)
    {
    
    }

    public override void Enter()
    {
        owner.GetPathing(owner.info.alertedLocation);
        owner.info.alerted = false;
    }

    public override bool Execute()
    {
        bool moving = owner.MoveCharacterAlongPath();

        if (moving)
        {

        }
        else if (!moving)
        {
            StartWait();
        }

        if (owner.sensor.Scan(LayerMask.GetMask("Player")))
        {
            waitingCoroutine = owner.StartAgentCoroutine(WaitInAlertedArea());
        }

        if (waitedInArea) return false;
        else return true;
    }

    public override void Exit()
    {
        owner.info.alertedLocation = Vector3.zero;
    }

    private void StartWait()
    {
        if (waitingCoroutine == null)
        {
            waitingCoroutine = owner.StartAgentCoroutine(WaitInAlertedArea());
        }
    }

    public IEnumerator WaitInAlertedArea()
    {
        yield return new WaitForSeconds(10f);

        waitedInArea = true;
    }
}
