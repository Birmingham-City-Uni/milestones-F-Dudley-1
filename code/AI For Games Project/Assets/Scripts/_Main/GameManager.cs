using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Village Locations")]
    public Transform guardLocationsContainer;
    public List<Transform> guardLocations = new List<Transform>();

    [Header("Debug Controllers")]
    [SerializeField] public bool drawNodeContainer = false;
    public static Action<bool> enableNodeContainerDrawing;

    [SerializeField] private bool drawPathing = false;
    public static Action<bool> enablePathDrawing;

    [SerializeField] private bool drawBoidTarget = false;
    public static Action<bool> enableBoidTargetDrawing;

    public bool DrawNodeContainer
    {
        get
        {
            return drawNodeContainer;
        }

        set
        {
            drawNodeContainer = value;
            enableNodeContainerDrawing(value);
        }
    }

    public bool DrawPathing
    {
        get
        {
            return drawPathing;
        }

        set
        {
            drawPathing = value;
            enablePathDrawing(value);
        }
    }

    public bool DrawBoidTarget
    {
        get
        {
            return drawBoidTarget;
        }
        set
        {
            drawBoidTarget = value;
            enableBoidTargetDrawing(value);
        }
    }

    #region Unity Functions
    private void Awake()
    {
        instance = this;

        foreach (Transform location in guardLocationsContainer)
        {
            guardLocations.Add(location);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            DrawPathing = !drawPathing;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            DrawNodeContainer = !drawNodeContainer;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            DrawBoidTarget = !drawBoidTarget;
        }
    }
    #endregion

    #region Guard Functions

    public Vector3 GetRandomGuardLocation() => guardLocations[UnityEngine.Random.Range(0, guardLocations.Count)].position;

    #endregion
}