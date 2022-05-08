using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class isTiredNode : BehaviourNode
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        public isTiredNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate() => (owner.info.Tiredness <= owner.info.tirednessThreshold) ? EvaluateState.SUCCESS : EvaluateState.FAILURE;

    }
}