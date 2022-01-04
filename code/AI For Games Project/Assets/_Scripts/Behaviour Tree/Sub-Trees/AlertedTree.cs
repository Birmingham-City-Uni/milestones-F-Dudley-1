using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class AlertedSubTree : Sequence
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        public AlertedSubTree(Agent _owner) : base(new List<BehaviourNode>())
        {
            owner = _owner;

            CreateAlertedTree();
        }

        public override EvaluateState Evaluate()
        {
            return base.Evaluate();
        }

        /// <summary>
        /// Constructs The Child Nodes of The Alerted SubTree.
        /// </summary>
        private void CreateAlertedTree()
        {
            GoToHouse goToHouseSequence = new GoToHouse(owner);
            
            // Construct SubTree
            childNodes.Add(new isAlertedNode(owner));
            childNodes.Add(goToHouseSequence);
        }
    }
}