using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SetJobLocationNode : BehaviourNode
    {
        Agent owner;
        Func<Transform> jobGetterFunction;

        public SetJobLocationNode(Agent _owner, Func<Transform> _jobFunction)
        {
            owner = _owner;
            jobGetterFunction = _jobFunction;
        }

        public override EvaluateState Evaluate()
        {
            if (!owner.info.hasJobLocation)
            {
                Debug.Log("Setting Job Location");
                owner.info.hasJobLocation = true;
                owner.info.currentJobLocation = jobGetterFunction();
                owner.GetPathing(owner.info.currentJobLocation.position);
            }

            nodeState = EvaluateState.SUCCESS;

            return nodeState;
        }
    }
}