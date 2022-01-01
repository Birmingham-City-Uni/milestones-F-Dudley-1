using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class TravelNode : BehaviourNode
    {
        Agent owner;
        Transform location;
        float distanceCheck;

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