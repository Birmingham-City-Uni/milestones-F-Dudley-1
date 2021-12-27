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

    protected void Update()
    {
        if (stateManager.hasState())
        {
            if (!stateManager.Update())
            {
                stateManager.popState();
            }
        }
        else stateManager.pushState(new PatrolState(this, stateManager));
    }

    protected new void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
    #endregion
}