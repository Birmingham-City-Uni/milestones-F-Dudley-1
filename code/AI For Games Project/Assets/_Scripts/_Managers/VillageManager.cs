using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour
{
    public static VillageManager instance;

    [Header("Key Locations")]
    [SerializeField] private Transform meetingLocation;
    [SerializeField] private Transform foodStall;
    
    [Header("Village Locations")]
    [SerializeField] private Transform guardLocationsContainer;
    [SerializeField] private List<Transform> guardLocations = new List<Transform>();

    private void Awake()
    {
        instance = this;

        if (guardLocationsContainer != null)
        {
            foreach (Transform location in guardLocationsContainer)
            {
                guardLocations.Add(location);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(foodStall.position, 1.75f);
    }

    #region Village Location Functions
    public Transform MeetingLocation
    {
        get
        {
            return meetingLocation;
        }
    }

    public Transform FoodStallLocation
    {
        get
        {
            return foodStall;
        }
    }

    public Vector3 GetRandomGuardLocation() => guardLocations[UnityEngine.Random.Range(0, guardLocations.Count)].position;
    #endregion
}
