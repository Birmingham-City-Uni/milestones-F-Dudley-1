using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree.Nondeterministic;

namespace BehaviourTree
{
    public class DoJobSubTree : Sequence
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        public DoJobSubTree(Agent _owner) : base(new List<BehaviourNode>())
        {
            owner = _owner;

            CreateJobTree();
        }

        public override EvaluateState Evaluate()
        {
            return base.Evaluate();
        }

        /// <summary>
        /// Constructs The ChildNodes of The Job SubTree.
        /// </summary>
        private void CreateJobTree()
        {
            // Go To Job
            NonDSelector getJobLocationNonDSelector = new NonDSelector(new List<BehaviourNode> { // Make Non - Deterministic
                
                new SetJobLocationNode(owner, owner.info.job.GetMainLocation),
                new SetJobLocationNode(owner, owner.info.job.GetSubLocation),
            });

            Selector checkJobLocation = new Selector(new List<BehaviourNode> {
                new hasJobLocationNode(owner),
                getJobLocationNonDSelector,
            });

            // Do Job
            Sequence doJobSequence = new Sequence(new List<BehaviourNode> {
                new StartJobNode(owner),
                new CompletedJobNode(owner),
            });

            // Add To Main Selector
            childNodes.Add(checkJobLocation);
            //childNodes.Add(new RangeNode(owner, owner.info.CurrentJobLocation, 1f));
            childNodes.Add(doJobSequence);
        }
    }
}