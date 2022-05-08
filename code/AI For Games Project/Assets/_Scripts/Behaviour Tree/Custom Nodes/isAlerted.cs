using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class isAlertedNode : BehaviourNode
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        public isAlertedNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate() => owner.info.isAlerted ? EvaluateState.SUCCESS : EvaluateState.FAILURE;

    }
}