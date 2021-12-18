using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingTester : MonoBehaviour
{
    [Header("Tester Variables")]
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 10.0f;
    public float waypointRange = 2.0f;
    public Transform target;

    [Space]

    [SerializeField] private Queue<Vector3> path = new Queue<Vector3>();

    [Header("Debug")]
    public bool showPathingGizmos;
    public float gizmoPathPointSize = 1.0f;

    #region Unity Functions
    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    void Update()
    {
        if (path.Count > 0)
        {
            transform.Translate((path.Peek() - transform.position) * moveSpeed * Time.deltaTime, Space.World);
            //transform.Rotate(0, Vector3.Angle(transform.forward, path.Peek() - transform.position), 0f, Space.World);

            if (Vector3.Distance(transform.position, path.Peek()) < waypointRange)
            {
                path.Dequeue();
            }
        }
    }

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
    private void OnPathFound(Vector3[] newWaypoints, bool pathingSuccess)
    {
        if (pathingSuccess)
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