using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Pathfinding Items
/// <summary>
/// A Struct Which Holds Neccassry Data For a Pathfinding Request To Be Finished.
/// </summary>
[System.Serializable]
public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    /// <summary>
    /// PathRequests Constructor.
    /// </summary>
    /// <param name="_start">The Pathfinding Requests Starting Location.</param>
    /// <param name="_end">The Pathfinding Requests Target Location.</param>
    /// <param name="_callback">The Pathfindin Requests Callback For After Processing.</param>
    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }
    #endregion
}