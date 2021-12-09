using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 _startPos, Vector3 _endPos, Action<Vector3[], bool> _callback)
    {
        pathStart = _startPos;
        pathEnd = _endPos;
        callback = _callback;
    }
}

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
            pathfinding.StartFindPath(currentRequest.pathStart, currentRequest.pathEnd);
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
