using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class CharacterInfo : MonoBehaviour
{
    [Header("Stats")]

    /// <summary>
    /// The Characters Health.
    /// </summary>
    [Range(0, 100)]
    [SerializeField] private int health;

    /// <summary>
    /// The Characters Gold.
    /// </summary>
    [SerializeField] private int gold;

    /// <summary>
    /// The Characters Hunger Value.
    /// </summary>
    [SerializeField] private float hunger;

    /// <summary>
    /// The Characters Tiredness Value.
    /// </summary>
    [SerializeField] private float tiredness;

    /// <summary>
    /// The Characters Hunger Value.
    /// </summary>
    /// <value>A Float of The Characters Hunger.</value>
    public float Hunger
    {
        get
        {
            return hunger;
        }
        set
        {
            hunger = Mathf.Clamp(value, 0, 100);
            hungerText.text = string.Format("Hunger: {0}", Mathf.RoundToInt(value));
        }
    }

    /// <summary>
    /// The Characters Tiredness Value.
    /// </summary>
    /// <value></value>
    public float Tiredness
    {
        get
        {
            return tiredness;
        }
        set
        {
            tiredness = Mathf.Clamp(value, 0, 100);
            tirednessText.text = string.Format("Tiredness: {0}", Mathf.RoundToInt(value));
        }
    }

    [Space]

    /// <summary>
    /// The Characters Alerted Value.
    /// </summary>
    [SerializeField] private bool alerted;

    /// <summary>
    /// The Recieved Location The Character Was Alerted From.
    /// </summary>
    [SerializeField] private Vector3 alertedLocation;

    /// <summary>
    /// The Characters Alerted Value.
    /// </summary>
    /// <value>A Bool For If The Character is Alerted or Not.</value>
    public bool isAlerted
    {
        get
        {
            return alerted;
        }
        set
        {
            alerted = value;
            isAlertedText.text = string.Format("isAlerted: {0}", value);
        }
    }

    /// <summary>
    /// The Location Where The Character Was Alerted From.
    /// </summary>
    /// <value></value>
    public Vector3 AlertedLocation
    {
        get
        {
            return alertedLocation;
        }

        set
        {
            isAlerted = true;
            alertedLocation = value;
        }
    }


    [Header("Job Stats")]

    /// <summary>
    /// The Knowledge of Wether The Character Currently has a Job Location.
    /// </summary>
    [SerializeField] private bool hasJobLocation = false;

    /// <summary>
    /// The Knowledge of Wether The Character Currently has Completed Their Assigned Job.
    /// </summary>
    /// <value></value>
    [SerializeField] private bool completedCurrentJob = false;

    /// <summary>
    /// The Position of Their Current Job Locaiton.
    /// </summary>
    [SerializeField] private Transform currentJobLocation;

    /// <summary>
    /// The Knowlege of Wether The Character has a Job Location.
    /// </summary>
    /// <value>A Bool, True if They Have a Location, False if They Have No Lcoation.</value>
    public bool HasJobLocation
    {
        get
        {
            return hasJobLocation;
        }

        set
        {
            hasJobLocation = value;
            hasJobLocationText.text = string.Format("HasJobLocation: {0}", value);
        }
    }

    /// <summary>
    /// The Knowledge of Wether The Character has Completed Their Current Assigned Job.
    /// </summary>
    /// <value>A Bool, True if They Have Completed Their Job, Fales if They Are Still Doing Their Job.</value>
    public bool CompletedCurrentJob
    {
        get
        {
            return completedCurrentJob;
        }
        set
        {
            completedCurrentJob = value;
            completedJobLocationText.text = string.Format("CompletedCurrentJob: {0}", value);
        }
    }

    /// <summary>
    /// The Current Location of Their Job.
    /// </summary>
    /// <value>A Transform of The Jobs Location.</value>
    public Transform CurrentJobLocation
    {
        get
        {
            return currentJobLocation;
        }
        set
        {
            currentJobLocation = value;
            if (value != null) currentJobLocationText.text = string.Format("(X: {0}, Y: {1}, Z: {2})", Mathf.RoundToInt(value.position.x), Mathf.RoundToInt(value.position.y), Mathf.RoundToInt(value.position.z));
        }
    }

    [Header("Effects Thresholds")]
    [Tooltip("The Amount at where the Player will start to Seek out Food")]
    /// <summary>
    /// The Threshold at When a Character Should Start Seeking out Food.
    /// </summary>
    public float hungryThreshold = 30f;

    [Tooltip("The Amount at where the Player will start to Go Home To Bed")]
    /// <summary>
    /// The Threshold When a Character Should Start Seeking out a Place to Sleep.
    /// </summary>
    public float tirednessThreshold = 30f;

    [Header("Enviroment References")]

    /// <summary>
    /// The Characters House.
    /// </summary>
    public CharacterHouse house;

    /// <summary>
    /// The Characters Job.
    /// </summary>
    public CharacterJob job;

    [Header("Visualizer References")]

    /// <summary>
    /// The Characters Debug Info Visualizer.
    /// </summary>
    [SerializeField] private GameObject debugVisualizer;

    /// <summary>
    /// The Debug CharacterInfos JobTitle Text.
    /// </summary>
    [SerializeField] private TextMeshProUGUI jobTitleText;

    /// <summary>
    /// The Debug CharacterInfos Hunger Text.
    /// </summary>
    [SerializeField] private TextMeshPro hungerText;

    /// <summary>
    /// The Debug CharacterInfos Tiredness Text.
    /// </summary>
    [SerializeField] private TextMeshPro tirednessText;

    /// <summary>
    /// The Debug CharacterInfos isAlerted Text.
    /// </summary>
    [SerializeField] private TextMeshPro isAlertedText;

    /// <summary>
    /// The Debug CharacterInfos hasJobLocation Text.
    /// </summary>
    [SerializeField] private TextMeshPro hasJobLocationText;

    /// <summary>
    /// The Debug CharacterInfos CompletedJobLocation Text.
    /// </summary>
    [SerializeField] private TextMeshPro completedJobLocationText;

    /// <summary>
    /// The Debug CharacterInfos CurrentJobLocation Text.
    /// </summary>
    [SerializeField] private TextMeshPro currentJobLocationText;

    #region Unity Functions

    /// <summary>
    /// Unitys Start Function.
    /// </summary>
    private void Start()
    {
        if (job != null) jobTitleText.text = job.JobName;
    }

    /// <summary>
    /// Unity OnEnable Function.
    /// </summary>
    private void OnEnable()
    {
        GameManager.enableCharacterInfoBoxes += ChangeVisualizerVisability;
    }

    /// <summary>
    /// Unitys OnDisable Function.
    /// </summary>
    private void OnDisable()
    {
        GameManager.enableCharacterInfoBoxes -= ChangeVisualizerVisability;
    }

    /// <summary>
    /// Unitys FixedUpdate Function. 
    /// </summary>
    private void FixedUpdate()
    {
        if (debugVisualizer != null)
        {
            if (debugVisualizer.activeSelf) debugVisualizer.transform.LookAt(Camera.main.transform.position, Vector3.up);            
        }
    }
    #endregion

    /// <summary>
    /// Changes The CharacterInfoBoxs Visability.
    /// </summary>
    /// <param name="_newVisability">The New Visalibty of The InfoBox.</param>
    private void ChangeVisualizerVisability(bool _newVisability)
    {
        if (debugVisualizer != null)
        {
            debugVisualizer.SetActive(_newVisability);            
        }
    }
}