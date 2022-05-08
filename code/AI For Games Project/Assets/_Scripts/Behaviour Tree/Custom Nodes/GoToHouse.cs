using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class GoToHouse : Sequence
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// The Range To Check For The Houses Entrance Location.
        /// </summary>
        const float rangeCheck = 2f;

        /// <summary>
        /// The Nodes Constructor
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        public GoToHouse(Agent _owner) : base(new List<BehaviourNode>())
        {
            owner = _owner;

            CreateGoToHouseSequence();
        }

        public override EvaluateState Evaluate()
        {
            return base.Evaluate();
        }

        /// <summary>
        /// Builds The Go To House Sequence's Nodes.
        /// </summary>
        private void CreateGoToHouseSequence()
        {
            childNodes.Add(new TravelNode(owner, owner.info.house.entrance, rangeCheck));
            childNodes.Add(new RangeNode(owner, owner.info.house.entrance, rangeCheck));
            childNodes.Add(new EnterHouseNode(owner));
        }
    }
}