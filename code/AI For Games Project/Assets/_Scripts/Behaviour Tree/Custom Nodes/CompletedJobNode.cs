using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class CompletedJobNode : BehaviourNode
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Current Node of The Owner.</param>
        public CompletedJobNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate()
        {
            if (owner.info.CompletedCurrentJob)
            {
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