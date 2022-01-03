using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class GoToHouse : Sequence
    {
        Agent owner;
        const float rangeCheck = 2f;

        public GoToHouse(Agent _owner) : base(new List<BehaviourNode>())
        {
            owner = _owner;

            CreateGoToHouseSequence();
        }

        public override EvaluateState Evaluate()
        {
            return base.Evaluate();
        }

        private void CreateGoToHouseSequence()
        {
            childNodes.Add(new TravelNode(owner, owner.info.house.entrance, rangeCheck));
            childNodes.Add(new RangeNode(owner, owner.info.house.entrance, rangeCheck));
            childNodes.Add(new EnterHouseNode(owner));
        }
    }
}