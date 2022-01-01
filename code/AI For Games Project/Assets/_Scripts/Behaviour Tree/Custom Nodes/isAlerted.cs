using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class isAlertedNode : BehaviourNode
    {
        Agent owner;

        public isAlertedNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate() => owner.info.isAlerted ? EvaluateState.SUCCESS : EvaluateState.FAILURE;

    }
}