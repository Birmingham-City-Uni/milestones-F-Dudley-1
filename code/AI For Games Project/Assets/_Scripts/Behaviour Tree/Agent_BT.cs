using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Agent_BT : Agent
{
    BehaviourNode topBehaviourNode;
    
    #region Unity Functions
    protected new void Start()
    {
        base.Start();

        ConstructBehaviourTree();
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

        topBehaviourNode.Evaluate();
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
        topBehaviourNode = new Selector(new List<BehaviourNode> {
            new AlertedSubTree(this),
            new HungrySubTree(this),
            new TiredSubTree(this),
            new DoJobSubTree(this),
        });
    }
    #endregion
}
