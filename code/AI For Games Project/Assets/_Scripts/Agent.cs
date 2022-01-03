using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(CharacterInfo))]
public class Agent : MonoBehaviour
{
    [Header("Base Agent")]
    [SerializeField] private Queue<Vector3> pathWaypoints = new Queue<Vector3>();
    public CharacterInfo info;
    private JobLocation currentJobLocation;
    public JobLocation CurrentJobLocation
    {
        get
        {
            return currentJobLocation;
        }
        set
        {
            currentJobLocation = value;
            // Redraw Debug Location Values.
        }
    }

    public Sensors sensor;
    private CharacterMovement controller;
    private LineRenderer pathRenderer;

    #region Unity Functions
    protected void Start()
    {
        info = GetComponent<CharacterInfo>();
        controller = GetComponent<CharacterMovement>();
        sensor = GetComponentInChildren<Sensors>();

        pathRenderer = GetComponentInChildren<LineRenderer>();

        info.Hunger = Random.Range(40, 100);
        info.Tiredness = Random.Range(50, 100);
    }

    protected void OnEnable()
    {
        GameManager.enablePathDrawing += DrawCharacterPathing;
    }

    protected void OnDisable()
    {
        GameManager.enablePathDrawing -= DrawCharacterPathing;
    }

    protected void Update()
    {
        MoveCharacterAlongPath();
    }

    protected void FixedUpdate()
    {
        info.Tiredness -= 0.005f;
        info.Hunger -= 0.0025f;
    }

    protected void OnDrawGizmos()
    {
        if (pathWaypoints.Count > 0 && GameManager.instance.DrawPathing)
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
    public float DistanceToTarget(Vector3 _target) => Vector3.Distance(transform.position, _target);

    public void ChangeAgentVisability(bool visability)
    {
        gameObject.SetActive(visability);
    }

    public void MoveCharacterAlongPath()
    {
        if (HasPath())
        {
            controller.UpdateCharacterPosition(pathWaypoints.Peek());

            if (Vector3.Distance(transform.position, pathWaypoints.Peek()) < 0.5f)
            {
                pathRenderer.SetPositions(pathWaypoints.ToArray());                
                pathWaypoints.Dequeue();
            }       
        }
        else {
            controller.StopCharacterAudio();
        }
    }

    public void MoveCharacterTowardsPoint(Vector3 _position)
    {
        controller.UpdateCharacterPosition(_position);
        if (Vector3.Distance(transform.position, _position) < 0.5f)
        {
            controller.StopCharacterAudio();
        }
    }

    protected void DrawCharacterPathing(bool isDrawing)
    {
        pathRenderer.enabled = isDrawing;
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

            pathRenderer.positionCount = newWaypoints.Length;
            pathRenderer.SetPositions(newWaypoints);
        }
        else Debug.Log("Agent Could Not Find Pathing");
    }
    #endregion
}