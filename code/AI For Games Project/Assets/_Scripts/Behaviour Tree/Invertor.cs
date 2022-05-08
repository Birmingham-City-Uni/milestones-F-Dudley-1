using System.Collections.Generic;

namespace BehaviourTree
{
    public class Invertor : BehaviourNode
    {
        /// <summary>
        /// The ChildNode To Invert The Values of.
        /// </summary>
        protected BehaviourNode childNode;

        /// <summary>
        /// The Nodes Constructor.
        /// </summary>
        /// <param name="_child">The Child To Invert The Evaluation of.</param>
        public Invertor(BehaviourNode _child)
        {
            childNode = _child;
        }

        public override EvaluateState Evaluate()
        {
            switch (childNode.Evaluate())
            {
                case EvaluateState.RUNNING:
                    nodeState = EvaluateState.RUNNING;
                    break;

                case EvaluateState.SUCCESS:
                    nodeState = EvaluateState.FAILURE;
                    break;

                case EvaluateState.FAILURE:
                    nodeState = EvaluateState.SUCCESS;
                    break;

                default:
                    break;
            }

            return nodeState;
        }
    }
}