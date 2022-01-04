using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class TravelNode : BehaviourNode
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// The Location To Travel To.
        /// </summary>
        Transform location;

        /// <summary>
        /// The Distance To Perform Travel Distance Checks At.
        /// </summary>
        float distanceCheck;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        /// <param name="_location">The Desired Location To Travel To.</param>
        /// <param name="_distance">The Distance To Check Travel.</param>
        public TravelNode(Agent _owner, Transform _location, float _distance)
        {
            owner = _owner;
            location = _location;

            distanceCheck = _distance;
        }

        public override EvaluateState Evaluate()
        {
            if (owner.DistanceToTarget(location.position) > distanceCheck && !owner.HasPath())
            {
                owner.GetPathing(location.position);
                nodeState = EvaluateState.RUNNING;
                return nodeState;
            }
            else
            {   
                nodeState = EvaluateState.SUCCESS;
                return nodeState;
            }
        }
    }
}