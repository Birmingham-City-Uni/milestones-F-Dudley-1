using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingTester : MonoBehaviour
{
    [Header("Tester Variables")]
    public Transform target;

    [Space]

    [SerializeField] private Vector3[] path;

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
        
    }

    private void OnDrawGizmos()
    {
        if (showPathingGizmos)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < path.Length; i++)
            {
                Gizmos.DrawSphere(path[i], gizmoPathPointSize);
                if (i < path.Length - 1)
                {
                    Gizmos.DrawLine(path[i], path[i + 1]);
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
            path = newWaypoints;
            Debug.Log("Pathing was Successful.");
        }
        else Debug.Log("Pathing Came Back Unsuccessful.");
    }
    #endregion
}
