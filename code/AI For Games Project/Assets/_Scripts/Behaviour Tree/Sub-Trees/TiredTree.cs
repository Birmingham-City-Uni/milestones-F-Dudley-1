using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class TiredSubTree : Sequence
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner"></param>
        /// <typeparam name="BehaviourNode"></typeparam>
        /// <returns></returns>
        public TiredSubTree(Agent _owner) : base(new List<BehaviourNode>())
        {
            owner = _owner;

            CreateTiredTree();
        }

        public override EvaluateState Evaluate()
        {
            return base.Evaluate();
        }

        /// <summary>
        /// Constructs ChildNodes of The Tired SubTree.
        /// </summary>
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