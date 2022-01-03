using System.Collections.Generic;

namespace BehaviourTree
{
    public class Selector : BehaviourNode
    {
        protected List<BehaviourNode> childNodes;

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