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
            if ((Vector3.Distance(owner.transform.position, owner.info.CurrentJobLocation.position) <= 2f) && !coroutineRunning && !owner.info.CompletedCurrentJob)
            {
                coroutineRunning = true;
                if (jobCoroutine != null) owner.StopCoroutine(jobCoroutine);
                jobCoroutine = owner.StartCoroutine(StartJobProcess());
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
            owner.SetAnimatorAnimations("isWorking", true);
            yield return new WaitForSeconds(6f);
            owner.SetAnimatorAnimations("isWorking", false);
            yield return new WaitForSeconds(9f);

            owner.info.CompletedCurrentJob = true;
            coroutineRunning = false;
        }
    }
}