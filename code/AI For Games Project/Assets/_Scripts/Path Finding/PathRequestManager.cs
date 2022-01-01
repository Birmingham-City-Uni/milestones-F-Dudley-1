using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    public static PathRequestManager instance;
    public Queue<PathRequest> requestQueue = new Queue<PathRequest>();
    private PathRequest currentRequest;

    private PathFinding pathfinding;

    public bool isProcessing;

    #region Unity Functions
    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PathFinding>();
    }
    #endregion

    #region Management Functions
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        instance.requestQueue.Enqueue(new PathRequest(pathStart, pathEnd, callback));
        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessing && requestQueue.Count > 0)
        {
            currentRequest = requestQueue.Dequeue();
            isProcessing = true;
            pathfinding.StartFindpath(currentRequest.pathStart, currentRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentRequest.callback(path, success);
        isProcessing = false;
        TryProcessNext();
    }
    #endregion
}
