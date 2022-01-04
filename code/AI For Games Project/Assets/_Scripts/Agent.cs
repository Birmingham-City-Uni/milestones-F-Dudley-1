using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(CharacterInfo))]
public class Agent : MonoBehaviour
{
    [Header("Base Agent")]

    /// <summary>
    /// A Queue of The Agents Current PathWaypoints.
    /// </summary>
    [SerializeField] private Queue<Vector3> pathWaypoints = new Queue<Vector3>();

    /// <summary>
    /// The Characters Info.
    /// </summary>
    public CharacterInfo info;

    /// <summary>
    /// The Current Job Location The Agent is Going To.
    /// </summary>
    private JobLocation currentJobLocation;

    /// <summary>
    /// The Current Job Location The Agent is Targeting.
    /// </summary>
    /// <value></value>
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

    /// <summary>
    /// The Agents Sensors.
    /// </summary>
    public Sensors sensor;

    /// <summary>
    /// The Agents Movement Controller.
    /// </summary>
    private CharacterMovement controller;

    /// <summary>
    /// The Agents Path Visualizer.
    /// </summary>
    private LineRenderer pathRenderer;

    /// <summary>
    /// Shows Console Debug Messages Assosiated With The Agent.
    /// </summary>
    public bool showDebugMessages;

    #region Unity Functions

    /// <summary>
    /// Unitys Start Function.
    /// </summary>
    protected void Start()
    {
        info = GetComponent<CharacterInfo>();
        controller = GetComponent<CharacterMovement>();
        sensor = GetComponentInChildren<Sensors>();

        pathRenderer = GetComponentInChildren<LineRenderer>();

        info.Hunger = Random.Range(40, 100);
        info.Tiredness = Random.Range(50, 100);
    }

    /// <summary>
    /// Unitys OnEnable Function.
    /// </summary>
    protected void OnEnable()
    {
        GameManager.enablePathDrawing += DrawCharacterPathing;
    }

    /// <summary>
    /// Unitys OnDisable Function.
    /// </summary>
    protected void OnDisable()
    {
        GameManager.enablePathDrawing -= DrawCharacterPathing;
    }

    /// <summary>
    /// Unitys Update Function.
    /// </summary>
    protected void Update()
    {
        MoveCharacterAlongPath();
    }

    /// <summary>
    /// Unitys FixedUpdate Function.
    /// </summary>
    protected void FixedUpdate()
    {
        info.Tiredness -= 0.005f;
        info.Hunger -= 0.0025f;
    }

    /// <summary>
    /// Unitys OnDrawGizmos Function.
    /// </summary>
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

    /// <summary>
    /// Checks if The Agent Currently Has a Path.
    /// </summary>
    /// <returns></returns>
    public bool HasPath() => pathWaypoints.Count > 0;

    /// <summary>
    /// Gets Pathing For The Agent From A Target Location.
    /// </summary>
    /// <param name="_targetLocation">The Target Location To Get Pathing To.</param>
    public void GetPathing(Vector3 _targetLocation) => PathRequestManager.RequestPath(transform.position, _targetLocation, FoundPathCallback);

    /// <summary>
    /// The Distance To The Target From The Agent.
    /// </summary>
    /// <param name="_target">The Agents Current Target.</param>
    /// <returns>Returns a Float of The Distance Between The Agent And Target.</returns>
    public float DistanceToTarget(Vector3 _target) => Vector3.Distance(transform.position, _target);

    /// <summary>
    /// Changes The Agents Visability.
    /// </summary>
    /// <param name="visability">The New Visability of The Agent.</param>
    public void ChangeAgentVisability(bool visability)
    {
        gameObject.SetActive(visability);
    }

    /// <summary>
    /// Moves The Agent Along Their Current Path, if they Have One.
    /// </summary>
    public void MoveCharacterAlongPath()
    {
        if (HasPath())
        {
            controller.UpdateCharacterPosition(pathWaypoints.Peek());
            controller.UpdateCharacterRotation(pathWaypoints.Peek());

            if (Vector3.Distance(transform.position, pathWaypoints.Peek()) < 0.5f)
            {
                pathRenderer.SetPositions(pathWaypoints.ToArray());
                pathWaypoints.Dequeue();
            }
        }
        else
        {
            controller.SetCharacterAnimations("isMoving", false);
            controller.StopCharacterAudio();
        }
    }

    /// <summary>
    /// Moves The Character Towards The Inputted Position.
    /// </summary>
    /// <param name="_position">The Position To Move Towards.</param>
    public void MoveCharacterTowardsPoint(Vector3 _position)
    {
        controller.UpdateCharacterPosition(_position);
        if (Vector3.Distance(transform.position, _position) < 0.5f)
        {
            controller.StopCharacterAudio();
        }
    }

    public void SetAnimatorAnimations(string animatorParameter, bool parameterValue)
    {
        controller.SetCharacterAnimations(animatorParameter, parameterValue);
    }

    /// <summary>
    /// Triigers Drawing of The Agents Pathing.
    /// </summary>
    /// <param name="isDrawing">The New Visablity of The Characters Pathing.</param>
    protected void DrawCharacterPathing(bool isDrawing)
    {
        pathRenderer.enabled = isDrawing;
    }

    /// <summary>
    /// The Agents Pathing Callback Which Assigns The Found Path To The Agent.
    /// </summary>
    /// <param name="newWaypoints">The New Waypoints of Pathfinding.</param>
    /// <param name="pathingSuccess">Was The Pathfinding Successful.</param>
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