using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class isHungryNode : BehaviourNode
    {
        Agent owner;

        public isHungryNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate() => (owner.info.hunger <= owner.info.hungryThreshold) ? EvaluateState.SUCCESS : EvaluateState.FAILURE;

    }
}