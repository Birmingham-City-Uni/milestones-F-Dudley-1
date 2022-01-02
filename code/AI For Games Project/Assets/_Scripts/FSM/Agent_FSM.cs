using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateManager))]
public class Agent_FSM : Agent
{
    [Header("Main Variables")]

    private StateManager stateManager;

    #region Unity Functions
    protected new void Start()
    {
        base.Start();
        stateManager = new StateManager();
        stateManager.Init(new PatrolState(this, stateManager));
    }

    protected new void OnEnable()
    {
        base.OnEnable();
    }

    protected new void OnDisable()
    {
        base.OnDisable();
    }

    protected new void Update()
    {
        MoveCharacterAlongPath();

        if (info.isAlerted)
        {
            stateManager.pushState(new SeekState(this, stateManager));
        }

        if (info.tiredness < info.tirednessThreshold)
        {
            Debug.Log("Agent is Tired");
            stateManager.pushState(new TiredState(this, stateManager));
        }

        /*
        else if (info.hunger < info.hungryThreshold)
        {
            stateManager.pushState(new HungryState(this, stateManager));
        }
        */

        if (stateManager.hasState())
        {
            if (!stateManager.Update())
            {
                stateManager.popState();
            }
        }
        else stateManager.pushState(new PatrolState(this, stateManager));
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected new void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, 12f);
    }
    #endregion
}