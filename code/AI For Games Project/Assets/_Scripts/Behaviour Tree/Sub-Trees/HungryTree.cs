using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class HungrySubTree : Sequence
    {
        Agent owner;

        public HungrySubTree(List<BehaviourNode> _childNodes, Agent _owner) : base(_childNodes)
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
            childNodes.Add(new isHungryNode(owner));
        }
    }
}