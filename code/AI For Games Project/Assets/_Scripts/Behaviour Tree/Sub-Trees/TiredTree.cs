using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class TiredSubTree : Sequence
    {
        Agent owner;

        public TiredSubTree(List<BehaviourNode> _childNodes, Agent _owner) : base(_childNodes)
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

        }
    }
}