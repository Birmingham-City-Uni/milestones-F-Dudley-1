using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// The Location of a Characters Job.
/// </summary>
public class JobLocation
{
    public string locationName;
    public Transform location;
}

public class CharacterJob : MonoBehaviour
{
    [Header("Job Description")]

    /// <summary>
    /// The Name of The Current Job.
    /// </summary>
    public string JobName;

    [Header("Job Locations")]

    /// <summary>
    /// The Main Location of The Job.
    /// </summary>
    [SerializeField] private JobLocation mainLocation;

    /// <summary>
    /// The Sub-Locations of The Job.
    /// </summary>
    [SerializeField] private JobLocation[] jobSubLocations;

    #region Unity Functions

    /// <summary>
    /// Unitys OnDrawGizmos Function.
    /// </summary>
    private void OnDrawGizmos()
    {
        const float rangeDetectionDistance = 2f;

        if (mainLocation != null)
        {
            Gizmos.DrawSphere(mainLocation.location.position, rangeDetectionDistance);
        }

        if (jobSubLocations.Length > 0)
        {
            foreach (JobLocation jobLocation in jobSubLocations)
            {
                if (jobLocation.location != null)
                {
                    Gizmos.DrawSphere(jobLocation.location.position, rangeDetectionDistance);                
                }
            }            
        }
    }
    #endregion

    #region Job Functions

    /// <summary>
    /// Gets The Jobs MainLocaiton.
    /// </summary>
    /// <returns>Returns a Vector3 of the Jobs Main Location.</returns>
    public Transform GetMainLocation() => mainLocation.location;

    /// <summary>
    /// Gets a Random Sub-Location.
    /// </summary>
    /// <returns>Returns a Vector3 of One of The Jobs Sub-Locations.</returns>
    public Transform GetSubLocation() => jobSubLocations[Random.Range(0, jobSubLocations.Length - 1)].location;
    #endregion
}
