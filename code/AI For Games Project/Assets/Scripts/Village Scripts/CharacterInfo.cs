using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInfo : MonoBehaviour
{
    [Header("Stats")]

    [Range(0, 100)]
    public int health;
    [Range(0, 100)]
    public int gold;

    [SerializeField] private bool alerted;
    [SerializeField] private Vector3 alertedLocation;

    [Space]

    public float hunger;
    public float tiredness;

    [Header("Enviroment References")]
    public CharacterHouse house;

    public bool isAlerted
    {
        get
        {
            return alerted;
        }

        set
        {
            alerted = value;
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
            alerted = true;
            alertedLocation = value;
        }
    }
}