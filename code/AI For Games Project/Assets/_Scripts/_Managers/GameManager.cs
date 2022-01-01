using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
            enableNodeContainerDrawing.Invoke(value);
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
            enablePathDrawing.Invoke(value);
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
            enableBoidTargetDrawing.Invoke(value);
        }
    }

    #region Unity Functions
    private void Awake()
    {
        instance = this;
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
}