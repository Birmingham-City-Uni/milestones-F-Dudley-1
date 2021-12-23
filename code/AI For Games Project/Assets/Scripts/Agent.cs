using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(CharacterInfo))]
public class Agent : MonoBehaviour
{
    [Header("Base Agent")]
    

    [SerializeField] private Queue<Vector3> pathWaypoints = new Queue<Vector3>();
    private CharacterInfo info;
    private Sensors sensor;
    private CharacterMovement controller;

    #region Unity Functions
    protected void Start()
    {
        info = GetComponent<CharacterInfo>();
        controller = GetComponent<CharacterMovement>();
        sensor = GetComponent<Sensors>();
    }

    protected void OnDrawGizmos()
    {
        if (pathWaypoints.Count > 0 && GameManager.instance.drawPathing)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, pathWaypoints.Peek());

            Vector3[] tmpPath = pathWaypoints.ToArray();
            for (int i = 0; i < pathWaypoints.Count; i++)
            {
                Gizmos.DrawWireSphere(tmpPath[i], 0.5f);
                if (i < pathWaypoints.Count - 1)
                {
                    Gizmos.DrawLine(tmpPath[i], tmpPath[i + 1]);
                }
            }
        }
    }
    #endregion

    #region Main Agent Functions
    public bool HasPath() => pathWaypoints.Count > 0;
    public void GetPathing(Vector3 _targetLocation) => PathRequestManager.RequestPath(transform.position, _targetLocation, FoundPathCallback);

    public void MoveCharacterAlongPath()
    {
        controller.UpdateCharacterPosition(pathWaypoints.Peek());

        if (Vector3.Distance(transform.position, pathWaypoints.Peek()) < 1f)
        {
            pathWaypoints.Dequeue();
        }
    }

    protected void FoundPathCallback(Vector3[] newWaypoints, bool pathingSuccess)
    {
        if (pathingSuccess)
        {
            pathWaypoints.Clear();

            foreach (Vector3 waypoint in newWaypoints)
            {
                pathWaypoints.Enqueue(waypoint);
            }
        }
        else Debug.Log("Agent Could Not Find Pathing");
    }
    #endregion
}