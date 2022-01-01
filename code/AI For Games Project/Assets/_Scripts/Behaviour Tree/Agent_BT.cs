using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_BT : Agent
{

    #region Unity Functions
    protected new void Awake()
    {
        base.Awake();
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
        base.Update();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected new void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
    #endregion

    #region Behaviour Tree Functions
    private void ConstructBehaviourTree()
    {

    }
    #endregion
}
