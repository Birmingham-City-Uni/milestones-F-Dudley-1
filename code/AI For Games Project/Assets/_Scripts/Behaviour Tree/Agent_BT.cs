using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Agent_BT : Agent
{
    /// <summary>
    /// The Top Node Of The Behaviour Tree.
    /// </summary>
    BehaviourNode topBehaviourNode;
    
    #region Unity Functions

    /// <summary>
    /// Unitys Start Function.
    /// </summary>
    protected new void Start()
    {
        base.Start();

        ConstructBehaviourTree();
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
        base.Update();

        topBehaviourNode.Evaluate();
    }

    /// <summary>
    /// Unitys FixedUpdate Function.
    /// </summary>
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    /// <summary>
    /// Unitys OnDrawGizmos Function.
    /// </summary>
    protected new void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
    #endregion

    #region Behaviour Tree Functions

    /// <summary>
    /// Constructs The ChildNodes of the Behaviour Tree.
    /// </summary>
    private void ConstructBehaviourTree()
    {
        topBehaviourNode = new Selector(new List<BehaviourNode> {
            new AlertedSubTree(this),
            new HungrySubTree(this),
            new TiredSubTree(this),
            new DoJobSubTree(this),
        });
    }
    #endregion
}
