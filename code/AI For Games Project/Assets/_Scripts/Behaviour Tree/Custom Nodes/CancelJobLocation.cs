using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class CancelJobLocationNode : BehaviourNode
    {
        Agent owner;

        public CancelJobLocationNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate()
        {
            owner.info.CompletedCurrentJob = false;
            owner.info.HasJobLocation = false;

            nodeState = EvaluateState.SUCCESS;
            return nodeState;
        }
    }
}