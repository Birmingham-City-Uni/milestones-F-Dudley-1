using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class DoJobSubTree : Sequence
    {
        Agent owner;

        public AlertedSubTree(Agent _owner) : base(new List<BehaviourNode>())
        {
            owner = _owner;

            CreateJobTree();
        }

        public override EvaluateState Evaluate()
        {
            return base.Evaluate();
        }

        private void CreateJobTree()
        {
            
        }
    }
}