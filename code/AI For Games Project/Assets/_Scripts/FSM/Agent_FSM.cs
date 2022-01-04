using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateManager))]
public class Agent_FSM : Agent
{
    [Header("Main Variables")]

    /// <summary>
    /// The StateManager For The Agent.
    /// </summary>
    private StateManager stateManager;

    #region Unity Functions
    /// <summary>
    /// Unitys Start Function.
    /// </summary>
    protected new void Start()
    {
        base.Start();
        stateManager = new StateManager();
        stateManager.Init(new PatrolState(this, stateManager));
    }

    /// <summary>
    /// Unitys OnEnable Function.
    /// </summary>
    protected new void OnEnable()
    {
        base.OnEnable();
    }

    /// <summary>
    /// Unitys OnDisable Function.
    /// </summary>
    protected new void OnDisable()
    {
        base.OnDisable();
    }

    /// <summary>
    /// Unitys Update Function.
    /// </summary>
    protected new void Update()
    {
        MoveCharacterAlongPath();

        if (info.isAlerted)
        {
            stateManager.pushState(new SeekState(this, stateManager));
        }

        if (info.Tiredness < info.tirednessThreshold)
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

    /// <summary>
    /// Unitys FixedUpdate Function.
    /// </summary>
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    /// <summary>
    /// Unitys FixedUpdate Function.
    /// </summary>
    protected new void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, 12f);
    }
    #endregion
}