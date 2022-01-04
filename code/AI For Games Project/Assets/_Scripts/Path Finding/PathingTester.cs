using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingTester : MonoBehaviour
{
    [Header("Tester Variables")]

    /// <summary>
    /// The Movement Speed The Pathing Should Be Tested.
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// The Rotation Speed of The Tester Going Along The Path.
    /// </summary>
    public float rotateSpeed = 10.0f;

    /// <summary>
    /// The Waypoint Range of The Tester, To Know if a Waypoint is Reached.
    /// </summary>
    public float waypointRange = 2.0f;

    /// <summary>
    /// The Target Node of The Tester, Which it Requests a Path To.
    /// </summary>
    public Transform target;

    [Space]

    /// <summary>
    /// The Queue of Positions to Run Through To The Target.
    /// </summary>
    [SerializeField] private Queue<Vector3> path = new Queue<Vector3>();

    [Header("Debug")]

    /// <summary>
    /// Determines if The Pathing Should Be Visualized Through Gizmos.
    /// </summary>
    public bool showPathingGizmos;

    /// <summary>
    /// The Size of The Visualized Nodes of The Pathing Gizmo.
    /// </summary>
    public float gizmoPathPointSize = 1.0f;

    #region Unity Functions

    /// <summary>
    /// Unitys Start Function.
    /// </summary>
    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    /// <summary>
    /// Unitys Update Function.
    /// </summary>
    void Update()
    {
        if (path.Count > 0)
        { 
            transform.position = Vector3.MoveTowards(transform.position, path.Peek(), moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, path.Peek()) < waypointRange)
            {
                path.Dequeue();
            }
        }
    }

    /// <summary>
    /// Unitys OnDrawGizmo Function.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (showPathingGizmos)
        {
            Gizmos.color = Color.yellow;

            Vector3[] tmpPath = path.ToArray();
            for (int i = 0; i < path.Count; i++)
            {
                Gizmos.DrawSphere(tmpPath[i], gizmoPathPointSize);
                if (i < path.Count - 1)
                {
                    Gizmos.DrawLine(tmpPath[i], tmpPath[i + 1]);
                }
            }
        }
    }
    #endregion

    #region Path Finding

    /// <summary>
    /// The Callback Used Once The PathRequestManager has a Path.
    /// </summary>
    /// <param name="newWaypoints">The New Path To The Requested Target.</param>
    /// <param name="pathingSuccess">Determines if The Path Was Successful.</param>
    private void OnPathFound(Vector3[] newWaypoints, bool pathingSuccess)
    {
        if (pathingSuccess) // If The Path is Successful, it Gets Adds To The Paths to the Queue.
        {
            foreach (Vector3 waypoint in newWaypoints)
            {
                path.Enqueue(waypoint);
            }
            Debug.Log("Pathing was Successful.");
        }
        else Debug.Log("Pathing Came Back Unsuccessful.");
    }
    #endregion
}