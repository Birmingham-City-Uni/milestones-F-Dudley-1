using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class isHungryNode : BehaviourNode
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        public isHungryNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate() => (owner.info.Hunger <= owner.info.hungryThreshold) ? EvaluateState.SUCCESS : EvaluateState.FAILURE;

    }
}