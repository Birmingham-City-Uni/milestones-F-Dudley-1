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
    public bool drawPathing;
    public bool drawBoidTarget;

    private void Awake()
    {
        instance = this;

        foreach (Transform location in guardLocationsContainer)
        {
            guardLocations.Add(location);
        }
    }

    #region Guard Functions

    public Vector3 GetRandomGuardLocation() => guardLocations[Random.Range(0, guardLocations.Count)].position;

    #endregion
}