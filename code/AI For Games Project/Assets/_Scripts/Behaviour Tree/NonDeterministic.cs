using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    namespace Nondeterministic
    {
        public class NonDSequence : Sequence
        {
            /// <summary>
            /// The Nodes Constructor.
            /// </summary>
            /// <param name="_childNodes">The ChildNodes of The Sequence.</param>
            /// <returns></returns>
            public NonDSequence(List<BehaviourNode> _childNodes) : base(_childNodes)
            {

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

                if (!isChildRunning) ShuffleChildren();

                return nodeState;
            }

            private void ShuffleChildren()
            {
                Debug.Log("Shuffling Child Nodes");
                childNodes = Utils.ShuffleList(childNodes);
            }
        }

        public class NonDSelector : Selector
        {
            /// <summary>
            /// The Nodes Constructor.
            /// </summary>
            /// <param name="_childNodes">The Selectors ChildNodes.</param>
            /// <returns></returns>
            public NonDSelector(List<BehaviourNode> _childNodes) : base(_childNodes)
            {

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
                            ShuffleChildren();
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

            private void ShuffleChildren()
            {
                Debug.Log("Shuffling Child Nodes");
                childNodes = Utils.ShuffleList(childNodes);
            }
        }
    }

}