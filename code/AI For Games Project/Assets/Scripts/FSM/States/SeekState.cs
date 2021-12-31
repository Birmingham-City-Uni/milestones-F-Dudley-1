using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{
    bool waitedInArea = false;
    bool waitingIsRunning = false;
    Coroutine waitingCoroutine;

    public SeekState(Agent _owner, StateManager _stateManager) : base(_owner, _stateManager)
    {
        waitedInArea = false;
        waitingIsRunning = false;
    }

    public override void Enter()
    {
        owner.GetPathing(owner.info.AlertedLocation);
        owner.info.isAlerted = false;
    }

    public override bool Execute()
    {   
        if (waitedInArea) return false;

        if (owner.MoveCharacterAlongPath())
        {

        }
        else if (!waitingIsRunning)
        {
            StartWait();
        }

        if (Physics.CheckSphere(owner.transform.position, 12f, LayerMask.GetMask("Player"), QueryTriggerInteraction.Ignore))
        {
            StartWait();
        }

        return true;
    }

    public override void Exit()
    {
        Debug.Log("Exiting Seek State");
    }

    private void StartWait()
    {
        waitingIsRunning = true;
        waitingCoroutine = owner.StartAgentCoroutine(WaitInAlertedArea());
    }

    public IEnumerator WaitInAlertedArea()
    {
        Debug.Log("Starting To Wait In Alerted Area");
        yield return new WaitForSeconds(20f);

        waitedInArea = true;
        waitingIsRunning = false;
    }
}
