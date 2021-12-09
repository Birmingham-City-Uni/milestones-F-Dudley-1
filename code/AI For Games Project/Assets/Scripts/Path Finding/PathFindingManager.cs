using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingManager : MonoBehaviour
{
    [Header("Main")]
    public static PathFindingManager instance;

    [SerializeField]
    private Queue<PathRequest> pathRequestsQueue = new Queue<PathRequest>();
    private PathRequest currentPathRequest;

    [Header("PathFinding")]
    public bool isProcessingPath;
    private PathFinding pathFinder;

    #region Unity Functions
    private void Awake()
    {
        instance = this;

        pathFinder = GetComponent<PathFinding>();
    }

    private void Update()
    {

    }
    #endregion

    #region Manager Functions
    public static void RequestPath(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(_pathStart, _pathEnd, callback);
        instance.pathRequestsQueue.Enqueue(newRequest);

        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath)
        {
            currentPathRequest = pathRequestsQueue.Dequeue();
            isProcessingPath = true;

            pathFinder.StartFindpath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedPathProcessing(Vector3[] _path, bool _success)
    {
        currentPathRequest.callback(_path, _success);
        isProcessingPath = false;
        instance.TryProcessNext();
    }
    #endregion
}
