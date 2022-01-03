using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlockManager : MonoBehaviour
{
    public static FlockManager instance;

    [Header("Main Variables")]
    [SerializeField] public GameObject birdPrefab;
    [SerializeField] private Transform boidsContainer;

    public static BoxCollider boundsCollider;

    [Header("Flock Attributes")]
    [SerializeField] public Vector3 flockBounds = new Vector3(0, 0, 0);
    [SerializeField] private float distanceFromGround = 10f;
    [SerializeField] private int targetChangeTime = 30;

    [SerializeField] public int numBirds = 100;
    [SerializeField] private Vector3 targetPosition = new Vector3(0f, 0f, 0f); 

    [Space]

    public GameObject[] birds;
    [SerializeField] private float timeChangeRemaining;
    private Coroutine targetChange;

    [Header("Debug")]
    public GameObject targetVisualizer;
    public bool drawFlockBounds;

    public Vector3 TargetPosition
    {
        get
        {
            return targetPosition;
        }
        set
        {
            targetPosition = value;
            MoveTargetVisualizer();
            GameManager.instance.ChangeBoidTargetText(value);
        }
    }

    private float TimeChangeRemaining
    {
        get
        {
            return timeChangeRemaining;
        }
        set
        {
            timeChangeRemaining = value;
            GameManager.instance.ChangeBoidSecondsRemainingText(value);
        }
    }

    #region Unity Functions
    private void Start()
    {
        instance = this;

        TargetPosition = GetPositionInBounds();
        boundsCollider = GetComponent<BoxCollider>();

        boundsCollider.center = new Vector3(0, distanceFromGround + (flockBounds.y / 2), 0);
        boundsCollider.size = flockBounds;

        birds = new GameObject[numBirds];
        for (int i = 0; i < numBirds; i++)
        {
            GameObject birdInstance = Instantiate(birdPrefab, GetPositionInBounds(), Quaternion.LookRotation(new Vector3(Random.Range(-1,1), 0, Random.Range(-1, 1))));
            birdInstance.transform.SetParent(boidsContainer);
            birds[i] = birdInstance;
        }

        targetChange = StartCoroutine(ChangeTarget());
    }

    private void OnEnable()
    {
        GameManager.enableBoidTargetDrawing += EnableVisualizer;
    }

    private void OnDisable()
    {
        GameManager.enableBoidTargetDrawing -= EnableVisualizer;
    }

    private void Update()
    {
        
    }

    private void OnTriggerExit(Collider collider)
    {   
        if (collider.gameObject.TryGetComponent<FlockAgent>(out FlockAgent agent))
        {
            agent.turning = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (drawFlockBounds)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3(0, distanceFromGround + (flockBounds.y / 2), 0), flockBounds);
        }

        if (GameManager.instance.DrawBoidTarget)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetPosition, 4f);
        }
    }
    #endregion

    #region Positioning Functions
    private IEnumerator ChangeTarget()
    {
        while (true)
        {
            for (TimeChangeRemaining = targetChangeTime; TimeChangeRemaining > 0; TimeChangeRemaining -= Time.deltaTime)
            {
                yield return null;
            }

            TargetPosition = GetPositionInBounds();            
        }
    }

    public Vector3 GetPositionInBounds()
    {
        Vector3 halfBounds = flockBounds * 0.3f;

        return new Vector3(Random.Range(-(halfBounds.x), halfBounds.x),
                           Random.Range(distanceFromGround, distanceFromGround + (flockBounds.y * 0.7f)), 
                           Random.Range(-(halfBounds.z), halfBounds.z));
    }

    public static bool CheckInBounds(Vector3 _positionToCheck)
    {
        return boundsCollider.bounds.Contains(_positionToCheck);
    }
    #endregion

    #region  Visualization Functions
    private void EnableVisualizer(bool isEnabled)
    {
        targetVisualizer.SetActive(isEnabled);
    }

    private void MoveTargetVisualizer()
    {
        targetVisualizer.transform.position = targetPosition;
    }
    #endregion

}