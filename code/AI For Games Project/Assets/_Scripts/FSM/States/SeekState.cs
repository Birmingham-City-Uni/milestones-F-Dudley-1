using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{
    /// <summary>
    /// Holds The Value if The Agent has Waited in The Alerted Area.
    /// </summary>
    bool waitedInArea = false;

    /// <summary>
    /// Holds The Value if The Waiting Coroutine is Running.
    /// </summary>
    bool waitingIsRunning = false;

    /// <summary>
    /// The Coroutine of The Waiting Sequence.
    /// </summary>
    Coroutine waitingCoroutine;

    /// <summary>
    /// The SeekStates Constructor.
    /// </summary>
    /// <param name="_owner">The States Owner.</param>
    /// <param name="_stateManager">The StateManager the Current State Belongs To.</param>
    /// <returns></returns>
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

        if (owner.DistanceToTarget(owner.info.AlertedLocation) <= 5f && !waitingIsRunning)
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

    /// <summary>
    /// Starts The Wait Coroutine.
    /// </summary>
    private void StartWait()
    {
        waitingIsRunning = true;
        if (waitingCoroutine != null) owner.StopCoroutine(waitingCoroutine);
        waitingCoroutine = owner.StartCoroutine(WaitInAlertedArea());
    }

    /// <summary>
    /// A Coroutine That Makes The Player Wait In The Alerted Area.
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitInAlertedArea()
    {
        Debug.Log("Starting To Wait In Alerted Area");
        yield return new WaitForSeconds(20f);

        waitedInArea = true;
        waitingIsRunning = false;
    }
}
