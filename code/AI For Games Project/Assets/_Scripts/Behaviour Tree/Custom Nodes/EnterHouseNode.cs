using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class EnterHouseNode : BehaviourNode
    {
        Agent owner;

        public EnterHouseNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate()
        {
            Debug.Log("Evaluating House Node");
            owner.info.house.EnterHouse(owner);
            nodeState = EvaluateState.SUCCESS;                

            return nodeState;
        }
    }
}