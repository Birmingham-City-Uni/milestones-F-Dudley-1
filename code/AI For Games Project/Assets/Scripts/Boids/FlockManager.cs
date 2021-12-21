using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlockManager : MonoBehaviour
{
    public static FlockManager instance;

    [Header("Main Variables")]
    public GameObject birdPrefab;
    public Transform player;
    public Transform boidsContainer;

    public static BoxCollider boundsCollider;

    [Header("Flock Attributes")]
    public Vector3 flockBounds = new Vector3(0, 0, 0);
    public float distanceFromGround = 10f;
    public float targetChangeTime = 30f;

    [SerializeField] public int numBirds = 100;
    [SerializeField] public Vector3 targetPosition = new Vector3(0f, 0f, 0f); 

    public GameObject[] birds;
    public Coroutine targetChange;

    [Header("Debug")]
    public bool drawFlockBounds;
    public bool drawTargetGizmo;

    #region Unity Functions
    private void Awake()
    {
        instance = this;

        targetPosition = GetPositionInBounds();
        boundsCollider = GetComponent<BoxCollider>();

        boundsCollider.center = new Vector3(0, distanceFromGround, 0);
        boundsCollider.size = flockBounds;

        birds = new GameObject[numBirds];
        for (int i = 0; i < numBirds; i++)
        {
            GameObject birdInstance = Instantiate(birdPrefab, GetPositionInBounds(), Quaternion.LookRotation(new Vector3(Random.Range(-1,1), 0, Random.Range(-1, 1))));
            birdInstance.GetComponent<FlockAgent>().SetPlayer(player);
            birdInstance.transform.SetParent(boidsContainer);
            birds[i] = birdInstance;
        }

        targetChange = StartCoroutine("ChangeTarget");
    }

    private void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (drawFlockBounds)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3(0, distanceFromGround + (flockBounds.y / 2), 0), flockBounds);
        }

        if (drawTargetGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetPosition, 4f);
        }
    }
    #endregion

    #region Positioning Functions
    private IEnumerator ChangeTarget()
    {
        yield return new WaitForSeconds(targetChangeTime);
        targetPosition = GetPositionInBounds();
    }

    public Vector3 GetPositionInBounds()
    {
        Vector3 halfBounds = flockBounds * 0.5f;

        return new Vector3(Random.Range(-(halfBounds.x), halfBounds.x),
                           Random.Range(distanceFromGround, distanceFromGround + flockBounds.y), 
                           Random.Range(-(halfBounds.z), halfBounds.z));
    }

    public static bool CheckInBounds(Vector3 _positionToCheck)
    {
        return boundsCollider.bounds.Contains(_positionToCheck);
    }
    #endregion
}
