using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInfo : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public int gold;

    public bool alerted;
    public Vector3 alertedLocation;

    [Space]

    public float hunger;
    public float tiredness;

    [Header("Enviroment References")]
    public Transform homeLocation;
}