using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree.Nondeterministic;

namespace BehaviourTree
{
    public class HungrySubTree : Sequence
    {
        Agent owner;

        public HungrySubTree(Agent _owner) : base(new List<BehaviourNode>())
        {
            owner = _owner;

            CreateHungryTree();
        }

        public override EvaluateState Evaluate()
        {
            return base.Evaluate();
        }

        private void CreateHungryTree()
        {
            // Go To Food Stall
            Sequence goToFoodStallSequence = new Sequence(new List<BehaviourNode> {
                new TravelNode(owner, VillageManager.instance.FoodStallLocation, 1.75f),
                new RangeNode(owner, VillageManager.instance.FoodStallLocation, 1.75f),
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