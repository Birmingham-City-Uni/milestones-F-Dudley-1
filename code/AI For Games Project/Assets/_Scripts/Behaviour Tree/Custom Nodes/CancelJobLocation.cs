using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class CancelJobLocationNode : BehaviourNode
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// Nodes Contructor.
        /// </summary>
        /// <param name="_owner">The Current Owner of The Node.</param>
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