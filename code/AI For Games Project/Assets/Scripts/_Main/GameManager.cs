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
    public bool drawGrid;
    public static Action enablePathNodesDrawing;

    public bool drawPathing;
    public static Action<bool> enablePathDrawing;

    public bool drawBoidTarget;
    public static Action<bool> enableBoidTargetDrawing;

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
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            enablePathDrawing.Invoke(true);
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            enablePathDrawing.Invoke(false);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            enablePathNodesDrawing.Invoke();
        }
    }
    #endregion

    #region Guard Functions

    public Vector3 GetRandomGuardLocation() => guardLocations[UnityEngine.Random.Range(0, guardLocations.Count)].position;

    #endregion
}