using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header("Main Variables")]
    [SerializeField] private Vector3[] pathWaypoints;

    private StateManager stateManager = new StateManager();
    private Sensors sensor;

    [Header("Agent Containers")]
    private Transform stateContainer;

    #region Unity Functions
    private void Start()
    {
        stateManager.Init(new WanderState(this, stateManager));
        sensor = GetComponent<Sensors>();

        if (stateContainer != null)
        {
            State[] foundStates = stateContainer.GetComponents<State>();

            if (foundStates.Length > 0)
            {
                foreach (State state in foundStates)
                {
                    stateManager.pushState(state);
                }
            }
        }
    }

    private void Update()
    {
        stateManager.Update();
        if ((sensor.Hit == true) && (stateManager.getCurrentState().GetType() != typeof(SeekState)))
        {
            //stateManager.pushState(seekState) // Push The Seek State Here AS Sensor Detects Something.
        }
        if ((sensor.Hit == false) && (stateManager.getCurrentState().GetType() != typeof(WanderState)))
        {
            //stateManager.pushState(wanderState); // Push The Wander State As Sensor Not Detected Anything.
        }
    }
    #endregion

    #region Main Agent Functions
    public void Move(Vector3 waypoint)
    {
        // Code To Interact With Character Controller Here.
    }

    protected void FoundPathCallback()
    {

    }
    #endregion
}