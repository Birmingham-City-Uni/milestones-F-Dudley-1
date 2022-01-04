using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class StartJobNode : BehaviourNode
    {
        /// <summary>
        /// The Nodes Owner.
        /// </summary>
        Agent owner;

        /// <summary>
        /// A Bool Which Checks if The Job Coroutine is Running.
        /// </summary>
        bool coroutineRunning = false;

        /// <summary>
        /// The Reference To The Currently Running Job Coroutine.
        /// </summary>
        Coroutine jobCoroutine = null;

        /// <summary>
        /// The Nodes Constructor
        /// </summary>
        /// <param name="_owner">The Owner Of The Behaviour Node.</param>
        public StartJobNode(Agent _owner)
        {
            owner = _owner;
        }

        public override EvaluateState Evaluate()
        {
            if (!coroutineRunning && !owner.info.CompletedCurrentJob)
            {
                Debug.Log("Started Coroutine");
                coroutineRunning = true;
                owner.StartCoroutine(StartJobProcess());
            }

            nodeState = EvaluateState.SUCCESS;
            return nodeState;
        }

        /// <summary>
        /// The Coroutine That Tracks The Jobs Progress.
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartJobProcess()
        {
            yield return new WaitForSeconds(10f);

            owner.info.CompletedCurrentJob = true;
            coroutineRunning = false;
        }
    }
}