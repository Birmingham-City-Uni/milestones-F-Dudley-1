using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class AlertedSubTree : Sequence
    {
        Agent owner;

        public AlertedSubTree(Agent _owner) : base(new List<BehaviourNode>())
        {
            owner = _owner;

            CreateAlertedTree();
        }

        public override EvaluateState Evaluate()
        {
            return base.Evaluate();
        }

        private void CreateAlertedTree()
        {
            Sequence goToHouseSequence = new Sequence(new List<BehaviourNode> {
                new TravelNode(owner, owner.info.house.entrance, 1.75f),
                new RangeNode(owner, owner.info.house.entrance, 1.75f),
                new EnterHouseNode(owner),
            });
            
            // Construct SubTree
            childNodes.Add(new isAlertedNode(owner));
            childNodes.Add(goToHouseSequence);
        }
    }
}