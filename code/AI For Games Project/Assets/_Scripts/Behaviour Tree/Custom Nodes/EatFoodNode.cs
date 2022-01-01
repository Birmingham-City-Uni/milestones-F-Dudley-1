using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class EatFoodNode : BehaviourNode
    {
        Agent owner;

        public EatFoodNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate()
        {
            if (owner.info.hunger <= owner.info.hungryThreshold)
            {
                owner.info.hunger = 100f;
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