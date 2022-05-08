using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SetJobLocationNode : BehaviourNode
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// The Function That is Used To Get A Job Location.
        /// </summary>
        Func<Transform> jobGetterFunction;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        /// <param name="_jobFunction">The Function That Returns a Job Location Transform.</param>
        public SetJobLocationNode(Agent _owner, Func<Transform> _jobFunction)
        {
            owner = _owner;
            jobGetterFunction = _jobFunction;
        }

        public override EvaluateState Evaluate()
        {
            if (!owner.info.HasJobLocation)
            {
                Debug.Log("Setting Job Location");
                owner.info.HasJobLocation = true;
                owner.info.CompletedCurrentJob = false;
                owner.info.CurrentJobLocation = jobGetterFunction();
                owner.GetPathing(owner.info.CurrentJobLocation.position);
            }

            nodeState = EvaluateState.SUCCESS;

            return nodeState;
        }
    }
}