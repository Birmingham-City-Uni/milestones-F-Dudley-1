using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// A Singleton References To the Current GameManager.
    /// </summary>
    public static GameManager instance;

    [Header("Canvas References")]
    /// <summary>
    /// The Debug Overlay GameObject.
    /// </summary>
    [SerializeField] private GameObject debugOverlay;

    /// <summary>
    /// The Debug Overlay Key's GameObject.
    /// </summary>
    [SerializeField] private GameObject debugOverlayKey;

    [Space]

    /// <summary>
    /// The GUI Element of The Boid Target Text.
    /// </summary>
    [SerializeField] private TextMeshProUGUI boidTargetLocationText;

    /// <summary>
    /// The GUI Element of The Boid Seconds Remaining Text.
    /// </summary>
    [SerializeField] private TextMeshProUGUI boidSecondsRemainingText;

    [Header("Debug Controllers")]

    /// <summary>
    /// DrawNodeContainer Bool that Triggers Visualization of The NodeContainer(s).
    /// </summary>
    [SerializeField] private bool drawNodeContainer = false;

    /// <summary>
    /// The Event / Action that Triggers Drawing of The Node Container Visualizer(s).
    /// </summary>
    public static Action<bool> enableNodeContainerDrawing;

    /// <summary>
    /// DrawInfoBox Bool that Triggers Drawing of InfoBoxes For Game Characters.
    /// </summary>
    [SerializeField] private bool drawInfoBox = false;

    /// <summary>
    /// The Event / Action that Triggers Drawing of Character InfoBoxes.
    /// </summary>
    public static Action<bool> enableCharacterInfoBoxes;

    /// <summary>
    /// DrawPathing Bool that Triggers Drawing of Agent Pathing.
    /// </summary>
    [SerializeField] private bool drawPathing = false;

    /// <summary>
    /// The Event / Action that Triggers Drawwing of Agent Pathing.
    /// </summary>
    public static Action<bool> enablePathDrawing;

    /// <summary>
    /// DrawBoidTarget Bool That Triggers Boid Target Drawing.
    /// </summary>
    [SerializeField] private bool drawBoidTarget = false;

    /// <summary>
    /// The Event / Action that Triggers Drawing of The Boid Target(s).
    /// </summary>
    public static Action<bool> enableBoidTargetDrawing;

    /// <summary>
    /// DrawNodeContainer Bool that Triggers Visualization of The NodeContainer(s).
    /// </summary>
    /// <value>A Bool That Triggers Drawing of The NodeContainer.</value>
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

    /// <summary>
    /// DrawInfoBox Bool that Triggers Drawing of InfoBoxes For Game Characters.
    /// </summary>
    /// <value>A Bool That Triggers Character InfoBox Drawing.</value>
    public bool DrawInfoBox
    {
        get
        {
            return drawInfoBox;
        }
        set
        {
            drawNodeContainer = value;
            enableCharacterInfoBoxes.Invoke(value);
        }
    }

    /// <summary>
    /// DrawPathing Bool that Triggers Drawing of Agent Pathing.
    /// </summary>
    /// <value>A Bool that Triggers Agent Path Drawing.</value>
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

    /// <summary>
    /// DrawBoidTarget Bool That Triggers Boid Target Drawing.
    /// </summary>
    /// <value>A Bool that Triggers Boid Target Drawing.</value>
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
    /// <summary>
    /// Unity Awake Function.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Unity Update Function.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            debugOverlayKey.SetActive(debugOverlay.activeSelf);
            debugOverlay.SetActive(!debugOverlay.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            DrawInfoBox = !drawInfoBox;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            DrawBoidTarget = !drawBoidTarget;
            
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            DrawPathing = !drawPathing;
        }        
        if (Input.GetKeyDown(KeyCode.M))
        {
            DrawNodeContainer = !drawNodeContainer;
        }
    }
    #endregion

    #region Debug Functions
    /// <summary>
    /// Changes the Boid Targets Text to the Inputted Parameter.
    /// </summary>
    /// <param name="_newLocation">The Desired Target for The Text.</param>
    public void ChangeBoidTargetText(Vector3 _newLocation)
    {
        boidTargetLocationText.text = String.Format("(X: {0}, Y: {1}, Z: {2})", Mathf.RoundToInt(_newLocation.x), Mathf.RoundToInt(_newLocation.y), Mathf.RoundToInt(_newLocation.z));
    }

    /// <summary>
    /// Changes The Boid Seconds Remaining Text to The Inputted parmeter.
    /// </summary>
    /// <param name="_newSeconds">The Desired Text For Seconds Remaining.</param>
    public void ChangeBoidSecondsRemainingText(float _newSeconds)
    {
        boidSecondsRemainingText.text = String.Format("{0} Seconds Left", Mathf.RoundToInt(_newSeconds));
    }
    #endregion
}