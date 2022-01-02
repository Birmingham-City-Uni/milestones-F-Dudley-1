using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class StartJobNode : BehaviourNode
    {
        Agent owner;
        bool coroutineRunning = false;
        Coroutine jobCoroutine = null;

        public StartJobNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate()
        {
            if (!coroutineRunning && !owner.info.completedCurrentJob)
            {
                Debug.Log("Started Coroutine");
                coroutineRunning = true;
                owner.StartCoroutine(StartJobProcess());
            }

            nodeState = EvaluateState.SUCCESS;
            return nodeState;
        }

        private IEnumerator StartJobProcess()
        {
            yield return new WaitForSeconds(10f);

            owner.info.completedCurrentJob = true;
            coroutineRunning = false;
        }
    }
}