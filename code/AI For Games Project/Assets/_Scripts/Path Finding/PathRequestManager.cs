using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    /// <summary>
    /// A Singleton Reference To Request Pathing From.
    /// </summary>
    public static PathRequestManager instance;

    /// <summary>
    /// The Current Queue of Pathfinding Requests To Complete.
    /// </summary>
    public Queue<PathRequest> requestQueue = new Queue<PathRequest>();

    /// <summary>
    /// The Current Pathfinding Request That is Being Processed.
    /// </summary>
    private PathRequest currentRequest;

    /// <summary>
    /// The Scenes Current Pathfinding Script.
    /// </summary>
    private PathFinding pathfinding;

    /// <summary>
    /// Determines if The PathRequest Manager is Currently Processing a Request.
    /// </summary>
    public bool isProcessing;

    #region Unity Functions

    /// <summary>
    /// Unitys Awake Function.
    /// </summary>
    private void Awake()
    {
        instance = this;

        pathfinding = GetComponent<PathFinding>();
    }
    #endregion

    #region Management Functions

    /// <summary>
    /// Requests a Path To Be Found, From Postion A To B.
    /// </summary>
    /// <param name="pathStart">The Starting Postion To Find a Path From.</param>
    /// <param name="pathEnd">The Target Positon To Find a Path To.</param>
    /// <param name="callback">The Agents Callback That Requested The Current Pathfinding.</param>
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        instance.requestQueue.Enqueue(new PathRequest(pathStart, pathEnd, callback));
        instance.TryProcessNext();
    }

    /// <summary>
    /// Prompts The Manager To TryPRocess The Next Request in The Queue.
    /// </summary>
    private void TryProcessNext()
    {
        if (!isProcessing && requestQueue.Count > 0)
        {
            currentRequest = requestQueue.Dequeue();
            isProcessing = true;
            pathfinding.StartFindpath(currentRequest.pathStart, currentRequest.pathEnd);
        }
    }

    /// <summary>
    /// Returns The Current Pathfinding Result To The Requested Agent.
    /// </summary>
    /// <param name="path">The Path Found From The Agent.</param>
    /// <param name="success">Was The Pathfinding Successful.</param>
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentRequest.callback(path, success); // Calling The Agents Callback To Send The Found Path Back.
        isProcessing = false;
        TryProcessNext();
    }
    #endregion
}
