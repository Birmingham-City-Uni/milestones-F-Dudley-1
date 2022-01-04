using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree.Nondeterministic;

namespace BehaviourTree
{
    public class HungrySubTree : Sequence
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        public HungrySubTree(Agent _owner) : base(new List<BehaviourNode>())
        {
            owner = _owner;

            CreateHungryTree();
        }

        public override EvaluateState Evaluate()
        {
            return base.Evaluate();
        }

        /// <summary>
        /// Constructs The ChildNodes of The Hungry SubTree.
        /// </summary>
        private void CreateHungryTree()
        {
            // Go To Food Stall
            Sequence goToFoodStallSequence = new Sequence(new List<BehaviourNode> {
                new TravelNode(owner, VillageManager.instance.FoodStallLocation, 2f),
                new RangeNode(owner, VillageManager.instance.FoodStallLocation, 2f),
                new EatFoodNode(owner),
            });

            // Go To House
            GoToHouse goToHouseSequence = new GoToHouse(owner);

            // Food Location Selector
            NonDSelector foodLocation = new NonDSelector(new List<BehaviourNode> {
                goToFoodStallSequence,
                goToHouseSequence,
            });
            
            // Construct SubTree
            childNodes.Add(new isHungryNode(owner));
            childNodes.Add(foodLocation);
        }
    }
}