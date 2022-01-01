using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                new TravelNode(owner, VillageManager.instance.FoodStallLocation, 2f),
                new RangeNode(owner, VillageManager.instance.FoodStallLocation, 2f),
                new EatFoodNode(owner),
            });

            // Go To House


            // Food Location Selector
            Selector foodLocation = new Selector(new List<BehaviourNode> {
                goToFoodStallSequence,
            });
            
            // Construct SubTree
            childNodes.Add(new isHungryNode(owner));
            childNodes.Add(foodLocation);
        }
    }
}