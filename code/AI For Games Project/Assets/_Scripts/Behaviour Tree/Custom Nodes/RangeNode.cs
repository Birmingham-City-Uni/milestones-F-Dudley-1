using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class RangeNode : BehaviourNode
    {
        private Agent owner;
        private Transform target;
        private float range;

        public RangeNode(Agent _owner, Transform _target, float _range)
        {
            owner = _owner;
            target = _target;
            range = _range;
        }

        public override EvaluateState Evaluate() => (Vector3.Distance(owner.gameObject.transform.position, target.position) <= range) ? EvaluateState.SUCCESS : EvaluateState.FAILURE; 
    }
}