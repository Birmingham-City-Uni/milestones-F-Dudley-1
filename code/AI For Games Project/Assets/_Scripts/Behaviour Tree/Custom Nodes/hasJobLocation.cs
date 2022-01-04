using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class hasJobLocationNode : BehaviourNode
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        private Agent owner;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        public hasJobLocationNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate() => (owner.info.HasJobLocation) ? EvaluateState.SUCCESS : EvaluateState.FAILURE; 
    }
}