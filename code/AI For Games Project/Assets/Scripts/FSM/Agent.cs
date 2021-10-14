using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header("Main Variables")]
    private StateManager stateManager = new StateManager();
    private Sensors sensor;

    /*
    [Header("State Variables")]
    Add Any States Here.
    */

    #region Unity Functions
    void Start()
    {
        // Call Contructors of Any States Here.

        stateManager.Init(new WanderState(this, stateManager));
        sensor = GetComponent<Sensors>();
    }

    void Update()
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
    public void Move(float speed, Vector3 waypoint)
    {
        // Code To Interact With Character Controller Here.
    }
    #endregion
}
