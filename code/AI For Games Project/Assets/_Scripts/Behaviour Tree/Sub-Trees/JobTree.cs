using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree.Nondeterministic;

namespace BehaviourTree
{
    public class DoJobSubTree : Selector
    {
        Agent owner;

        public DoJobSubTree(Agent _owner) : base(new List<BehaviourNode>())
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
            // Go To Job
            NonDSelector getJobLocationNonDSelector = new NonDSelector(new List<BehaviourNode> { // Make Non - Deterministic
                
                new SetJobLocationNode(owner, owner.info.job.GetMainLocation),
                new SetJobLocationNode(owner, owner.info.job.GetSubLocation),
            });

            Selector checkJobLocation = new Selector(new List<BehaviourNode> {
                new hasJobLocationNode(owner),
                getJobLocationNonDSelector,
            });

            Sequence goToJobLocationSequence = new Sequence(new List<BehaviourNode> {
                checkJobLocation,
                new RangeNode(owner, owner.info.CurrentJobLocation, 2f),
            });

            // Do Job
            Sequence doJobSequence = new Sequence(new List<BehaviourNode> {
                new StartJobNode(owner),
                new CompletedJobNode(owner),
                new CancelJobLocationNode(owner),
            });

            // Add To Main Selector
            childNodes.Add(goToJobLocationSequence);
            childNodes.Add(doJobSequence);
        }
    }
}