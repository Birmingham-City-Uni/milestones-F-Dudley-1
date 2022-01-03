using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class TiredSubTree : Sequence
    {
        Agent owner;

        public TiredSubTree(Agent _owner) : base(new List<BehaviourNode>())
        {
            owner = _owner;

            CreateTiredTree();
        }

        public override EvaluateState Evaluate()
        {
            return base.Evaluate();
        }

        private void CreateTiredTree()
        {
            // Go To House Sequence
            GoToHouse goToHouseSequence = new GoToHouse(owner);

            // Is Tired Checker then GoToHouseSequence
            childNodes.Add(new isTiredNode(owner));
            childNodes.Add(goToHouseSequence);
        }
    }
}