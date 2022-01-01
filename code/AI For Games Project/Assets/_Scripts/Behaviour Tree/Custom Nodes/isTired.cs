using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class isTiredNode : BehaviourNode
    {
        Agent owner;

        public isTiredNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate() => (owner.info.tiredness <= owner.info.tirednessThreshold) ? EvaluateState.SUCCESS : EvaluateState.FAILURE;

    }
}