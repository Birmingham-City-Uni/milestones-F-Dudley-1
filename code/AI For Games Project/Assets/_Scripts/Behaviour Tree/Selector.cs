using System.Collections.Generic;

namespace BehaviourTree
{
    public class Selector : BehaviourNode
    {
        /// <summary>
        /// The List of ChildNodes The Selector Has.
        /// </summary>
        protected List<BehaviourNode> childNodes;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_childNodes">The Desired ChildNodes of The Selector.</param>
        public Selector(List<BehaviourNode> _childNodes)
        {
            childNodes = _childNodes;
        }

        public override EvaluateState Evaluate()
        {
            foreach (BehaviourNode child in childNodes)
            {
                switch (child.Evaluate())
                {
                    case EvaluateState.RUNNING:
                        nodeState = EvaluateState.RUNNING;
                        return nodeState;

                    case EvaluateState.SUCCESS:
                        nodeState = EvaluateState.SUCCESS;
                        return nodeState;

                    case EvaluateState.FAILURE:
                        break;

                    default:
                        break;
                }
            }

            nodeState = EvaluateState.FAILURE;
            return nodeState;
        }
    }
}