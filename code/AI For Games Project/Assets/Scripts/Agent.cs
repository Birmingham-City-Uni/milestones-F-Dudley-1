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
    }

    protected void OnEnable()
    {
        GameManager.enablePathDrawing += DrawCharacterPathing;
    }

    protected void OnDisable()
    {
        GameManager.enablePathDrawing -= DrawCharacterPathing;
    }

    protected void FixedUpdate()
    {
        info.tiredness -= 0.005f;
        info.hunger -= 0.0025f;

        info.tiredness = Mathf.Clamp(info.tiredness, 0, 100f);
        info.hunger = Mathf.Clamp(info.hunger, 0, 100);
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
    public Coroutine StartAgentCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);

    public void ChangeAgentVisability(bool visability)
    {
        gameObject.SetActive(visability);
    }

    public bool MoveCharacterAlongPath()
    {
        if (HasPath())
        {
            controller.UpdateCharacterPosition(pathWaypoints.Peek());

            if (Vector3.Distance(transform.position, pathWaypoints.Peek()) < 0.5f)
            {
                pathWaypoints.Dequeue();
            }      

            return true;      
        }
        else {
            controller.StopCharacterAudio();

            return false;
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