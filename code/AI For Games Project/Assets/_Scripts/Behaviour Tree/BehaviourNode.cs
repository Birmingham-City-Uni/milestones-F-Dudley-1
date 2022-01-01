using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum EvaluateState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    [System.Serializable]
    public abstract class BehaviourNode
    {
        protected EvaluateState nodeState;
        public EvaluateState NodeState 
        {
            get
            {
                return nodeState;
            }
        }

        public abstract EvaluateState Evaluate();
    }    
}