using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class CompletedJobNode : BehaviourNode
    {
        Agent owner;

        public CompletedJobNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate()
        {
            if (owner.info.completedCurrentJob)
            {
                Debug.Log("Completed Current Job");
                nodeState = EvaluateState.SUCCESS;
            }
            else 
            {
                nodeState = EvaluateState.RUNNING;                
            }

            return nodeState;
        }
    }
}