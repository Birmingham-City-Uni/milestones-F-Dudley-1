using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class EatFoodNode : BehaviourNode
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Current Owner of The Behaviour Node.</param>
        public EatFoodNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate()
        {
            if (owner.info.Hunger <= owner.info.hungryThreshold)
            {
                owner.info.Hunger = 100f;
                nodeState = EvaluateState.SUCCESS;
            }
            else
            {
                nodeState = EvaluateState.FAILURE;
            }

            return nodeState;
        }
    }
}