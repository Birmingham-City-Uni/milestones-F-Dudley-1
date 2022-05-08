using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour
{
    /// <summary>
    /// A Singleton Reference To The VillageManager Instance in The GameScene.
    /// </summary>
    public static VillageManager instance;

    [Header("Key Locations")]

    /// <summary>
    /// The Transform of The Villages Meeting Location.
    /// </summary>
    [SerializeField] private Transform meetingLocation;

    /// <summary>
    /// The Transform of The Villages FoodStall.
    /// </summary>
    [SerializeField] private Transform foodStall;
    
    [Header("Village Locations")]

    /// <summary>
    /// The Container That Holds The Guards Patrolling Location.
    /// </summary>
    [SerializeField] private Transform guardLocationsContainer;

    /// <summary>
    /// A List Containing All The Guards Patrolling Locations.
    /// </summary>
    [SerializeField] private List<Transform> guardLocations = new List<Transform>();

    #region Unity Functions

    /// <summary>
    /// Unity Awake Function.
    /// </summary>
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

    /// <summary>
    /// Unity OnDrawGizmos Function.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(foodStall.position, 1.75f);
    }
    #endregion

    #region Village Location Functions

    /// <summary>
    /// The Villages Meeting Location.
    /// </summary>
    /// <value>A Transform of the Villages Meeting Location.</value>
    public Transform MeetingLocation
    {
        get
        {
            return meetingLocation;
        }
    }

    /// <summary>
    /// The Villages Food Stall Location.
    /// </summary>
    /// <value>A Transform of The Villages Food Stall Location.</value>
    public Transform FoodStallLocation
    {
        get
        {
            return foodStall;
        }
    }

    /// <summary>
    /// Gets a Random Guard Location From The List of Guard Locations.
    /// </summary>
    /// <returns>A Vector3 of a Guard Patrol Location.</returns>
    public Vector3 GetRandomGuardLocation() => guardLocations[UnityEngine.Random.Range(0, guardLocations.Count)].position;
    #endregion
}
