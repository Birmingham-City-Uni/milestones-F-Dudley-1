using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class hasJobLocationNode : BehaviourNode
    {
        private Agent owner;

        public hasJobLocationNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate() => (owner.info.hasJobLocation) ? EvaluateState.SUCCESS : EvaluateState.FAILURE; 
    }
}