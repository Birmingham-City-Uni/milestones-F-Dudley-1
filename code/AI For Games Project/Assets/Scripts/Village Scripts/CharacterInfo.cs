using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInfo : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public int gold;

    [Space]

    public float hunger;
    public float tiredness;

    [Header("Enviroment References")]
    public Transform homeLocation;
}