using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class RangeNode : BehaviourNode
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        private Agent owner;

        /// <summary>
        /// The Target To Range Check.
        /// </summary>
        private Transform target;

        /// <summary>
        /// The Range At Where The Target is In Range.
        /// </summary>
        private float range;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        /// <param name="_target">The Target Transform To Check.</param>
        /// <param name="_range">The Distance At Where The Target Is In Range.</param>
        public RangeNode(Agent _owner, Transform _target, float _range)
        {
            owner = _owner;
            target = _target;
            range = _range;
        }

        public override EvaluateState Evaluate()
        {
            if (owner.showDebugMessages) Debug.Log("Current Range:" + Vector3.Distance(owner.gameObject.transform.position, target.position));

            return (Vector3.Distance(owner.gameObject.transform.position, target.position) <= range) ? EvaluateState.SUCCESS : EvaluateState.FAILURE;
        }
    }
}