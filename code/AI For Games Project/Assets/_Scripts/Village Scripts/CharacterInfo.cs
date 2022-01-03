using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class CharacterInfo : MonoBehaviour
{
    [Header("Stats")]

    [Range(0, 100)]
    [SerializeField] private int health;
    [SerializeField] private int gold;

    [SerializeField] private float hunger;
    [SerializeField] private float tiredness;

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

    [SerializeField] private bool alerted;
    [SerializeField] private Vector3 alertedLocation;

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
    [SerializeField] private bool hasJobLocation = false;
    [SerializeField] private bool completedCurrentJob = false;
    [SerializeField] private Transform currentJobLocation;

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

    public Transform CurrentJobLocation
    {
        get
        {
            return currentJobLocation;
        }
        set
        {
            currentJobLocation = value;
            currentJobLocationText.text = string.Format("(X: {0}, Y: {1}, Z: {2})", Mathf.RoundToInt(value.position.x), Mathf.RoundToInt(value.position.y), Mathf.RoundToInt(value.position.z));
        }
    }

    [Header("Effects Thresholds")]
    [Tooltip("The Amount at where the Player will start to Seek out Food")]
    public float hungryThreshold = 30f;
    [Tooltip("The Amount at where the Player will start to Go Home To Bed")]
    public float tirednessThreshold = 30f;

    [Header("Enviroment References")]
    public CharacterHouse house;
    public CharacterJob job;

    [Header("Visualizer References")]
    [SerializeField] private GameObject debugVisualizer;
    [SerializeField] private TextMeshProUGUI jobTitleText;
    [SerializeField] private TextMeshPro hungerText;
    [SerializeField] private TextMeshPro tirednessText;
    [SerializeField] private TextMeshPro isAlertedText;
    [SerializeField] private TextMeshPro hasJobLocationText;
    [SerializeField] private TextMeshPro completedJobLocationText;
    [SerializeField] private TextMeshPro currentJobLocationText;

    private void Start()
    {
        if (job != null) jobTitleText.text = job.JobName;
    }

    private void OnEnable()
    {
        GameManager.enableCharacterInfoBoxes += ChangeVisualizerVisability;
    }

    private void OnDisable()
    {
        GameManager.enableCharacterInfoBoxes -= ChangeVisualizerVisability;
    }

    private void FixedUpdate()
    {
        if (debugVisualizer != null)
        {
            if (debugVisualizer.activeSelf) debugVisualizer.transform.LookAt(Camera.main.transform.position, Vector3.up);            
        }
    }

    private void ChangeVisualizerVisability(bool _newVisability)
    {
        if (debugVisualizer != null)
        {
            debugVisualizer.SetActive(_newVisability);            
        }
    }
}