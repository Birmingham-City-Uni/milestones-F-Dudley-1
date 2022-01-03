using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Canvas References")]
    [SerializeField] private GameObject debugOverlay;
    [SerializeField] private GameObject debugOverlayKey;

    [Space]

    [SerializeField] private TextMeshProUGUI boidTargetLocationText;
    [SerializeField] private TextMeshProUGUI boidSecondsRemainingText;

    [Header("Debug Controllers")]
    [SerializeField] private bool drawNodeContainer = false;
    public static Action<bool> enableNodeContainerDrawing;

    [SerializeField] private bool drawInfoBox = false;
    public static Action<bool> enableCharacterInfoBoxes;

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
        if (Input.GetKeyDown(KeyCode.V))
        {
            debugOverlayKey.SetActive(debugOverlay.activeSelf);
            debugOverlay.SetActive(!debugOverlay.activeSelf);
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
    public void ChangeBoidTargetText(Vector3 _newLocation)
    {
        boidTargetLocationText.text = String.Format("(X: {0}, Y: {1}, Z: {2})", Mathf.RoundToInt(_newLocation.x), Mathf.RoundToInt(_newLocation.y), Mathf.RoundToInt(_newLocation.z));
    }

    public void ChangeBoidSecondsRemainingText(float _newSeconds)
    {
        boidSecondsRemainingText.text = String.Format("{0} Seconds Left", Mathf.RoundToInt(_newSeconds));
    }
    #endregion
}