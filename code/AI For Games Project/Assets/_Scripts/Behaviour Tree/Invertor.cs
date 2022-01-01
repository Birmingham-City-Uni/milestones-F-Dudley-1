using System.Collections.Generic;

namespace BehaviourTree
{
    public class Invertor : BehaviourNode
    {
        protected BehaviourNode childNode;

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