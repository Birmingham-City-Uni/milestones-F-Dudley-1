using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class TravelNode : BehaviourNode
    {
        Agent owner;
        Transform location;

        public TravelNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate()
        {
            if (!owner.HasPath() && nodeState != EvaluateState.RUNNING) {
                owner.GetPathing(location.position);
                nodeState = EvaluateState.RUNNING;
                return nodeState;
            }
            else
            {
                nodeState = EvaluateState.SUCCESS;
            }

            return nodeState;
        }
    }
}