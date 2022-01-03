using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JobLocation
{
    public string locationName;
    public Transform location;
}

public class CharacterJob : MonoBehaviour
{
    [Header("Job Description")]
    public string JobName;

    [Header("Job Locations")]
    [SerializeField] private JobLocation mainLocation;
    [SerializeField] private JobLocation[] jobSubLocations;

    #region Unity Functions
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
    public Transform GetMainLocation() => mainLocation.location;
    public Transform GetSubLocation() => jobSubLocations[Random.Range(0, jobSubLocations.Length - 1)].location;
    #endregion
}
