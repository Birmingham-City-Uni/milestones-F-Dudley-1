using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterStats
{
    public int health;
    public int gold;

    public float hunger;    
    public float tiredness;

}

public class CharacterInfo : MonoBehaviour
{
    public CharacterStats stats;

    public Transform homeLocation;
}
