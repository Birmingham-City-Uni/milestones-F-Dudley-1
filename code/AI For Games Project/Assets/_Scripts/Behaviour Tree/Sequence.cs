using System.Collections.Generic;

namespace BehaviourTree
{
    public class Sequence : BehaviourNode
    {
        protected List<BehaviourNode> childNodes;

        
        public Sequence(List<BehaviourNode> _childNodes)
        {
            childNodes = _childNodes;
        }

        public override EvaluateState Evaluate()
        {
            bool isChildRunning = false;

            foreach (BehaviourNode child in childNodes)
            {
                switch (child.Evaluate())
                {
                    case EvaluateState.RUNNING:
                        isChildRunning = true;
                        break;

                    case EvaluateState.SUCCESS:
                        break;

                    case EvaluateState.FAILURE:
                        nodeState = EvaluateState.FAILURE;
                        return nodeState;

                    default:
                        break;
                }
            }

            nodeState = isChildRunning ? EvaluateState.RUNNING : EvaluateState.SUCCESS;

            return nodeState;
        }
    }    
}