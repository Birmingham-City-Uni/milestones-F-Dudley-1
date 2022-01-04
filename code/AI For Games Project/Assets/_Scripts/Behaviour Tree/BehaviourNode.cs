using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    /// <summary>
    /// The Evaluation States of BehaviourNodes.
    /// </summary>
    public enum EvaluateState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    [System.Serializable]
    public abstract class BehaviourNode
    {
        /// <summary>
        /// The Current State of The Behaviour Node.
        /// </summary>
        protected EvaluateState nodeState;

        /// <summary>
        /// The Current State of The Behaviour Node.
        /// </summary>
        /// <value></value>
        public EvaluateState NodeState
        {
            get
            {
                return nodeState;
            }
        }

        /// <summary>
        /// Evaluates The Node.
        /// </summary>
        /// <returns>The Current State of The Node.</returns>
        public abstract EvaluateState Evaluate();
    }
}