using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlockManager : MonoBehaviour
{
    public static FlockManager instance;

    [Header("Main Variables")]

    /// <summary>
    /// The Prefab of The Flocking Boid.
    /// </summary>
    [SerializeField] public GameObject birdPrefab;

    /// <summary>
    /// The Container That Will Hold All The Instantiated Boids Objects.
    /// </summary>
    [SerializeField] private Transform boidsContainer;

    /// <summary>
    /// The Flocking Bounds Collider.
    /// </summary>
    public static BoxCollider boundsCollider;

    [Header("Flock Attributes")]
    [SerializeField] public Vector3 flockBounds = new Vector3(0, 0, 0);
    [SerializeField] private float distanceFromGround = 10f;
    [SerializeField] private int targetChangeTime = 30;

    /// <summary>
    /// The Number of Birds Generated.
    /// </summary>
    [SerializeField] public int numBirds = 100;

    /// <summary>
    /// The Target Position of the Boids.
    /// </summary>
    /// <returns></returns>
    [SerializeField] private Vector3 targetPosition = new Vector3(0f, 0f, 0f); 

    [Space]

    /// <summary>
    /// The Currently Instaniated Birds.
    /// </summary>
    public GameObject[] birds;

    /// <summary>
    /// The Current Time Remaining Until The Boid Target Change.
    /// </summary>
    [SerializeField] private float timeChangeRemaining;

    /// <summary>
    /// The Coroutine Controlling The Target Change.
    /// </summary>
    private Coroutine targetChange;

    [Header("Debug")]
    /// <summary>
    /// The Debug Visualizer GameObject.
    /// </summary>
    public GameObject targetVisualizer;
    public bool drawFlockBounds;

    /// <summary>
    /// The Target Position of the Boids.
    /// </summary>
    /// <value>A Vector3 of The Boid Target Position in World Space.</value>
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

    /// <summary>
    /// The Current Time Remaining Until The Boid Target Change.
    /// </summary>
    /// <value>A Float of The Time Current Time Remaining.</value>
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
    /// <summary>
    /// Unity Start Function.
    /// </summary>
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

    /// <summary>
    /// Unity OnEnable Function.
    /// </summary>
    private void OnEnable()
    {
        GameManager.enableBoidTargetDrawing += EnableVisualizer;
    }

    /// <summary>
    /// Unity OnDisable Function.
    /// </summary>
    private void OnDisable()
    {
        GameManager.enableBoidTargetDrawing -= EnableVisualizer;
    }

    /// <summary>
    /// Unity Update Function.
    /// </summary>
    private void Update()
    {
        
    }

    /// <summary>
    /// Unity OnTriggerExit Function.
    /// </summary>
    /// <param name="collider">The Collider That Left The Trigger's Bounds.</param>
    private void OnTriggerExit(Collider collider)
    {   
        if (collider.gameObject.TryGetComponent<FlockAgent>(out FlockAgent agent))
        {
            agent.turning = true;
        }
    }

    /// <summary>
    /// Unity OnDrawGizmos Function.
    /// </summary>
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
    /// <summary>
    /// A Coroutine to Change The Boids Target Position Periodically.
    /// </summary>
    /// <returns>A TargetChange Coroutine.</returns>
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

    /// <summary>
    /// Gets a Position In The Flocks Air Bounds.
    /// </summary>
    /// <returns>Returns a Position Located In The Flocking Bounds.</returns>
    public Vector3 GetPositionInBounds()
    {
        Vector3 halfBounds = flockBounds * 0.3f;

        return new Vector3(Random.Range(-(halfBounds.x), halfBounds.x),
                           Random.Range(distanceFromGround, distanceFromGround + (flockBounds.y * 0.7f)), 
                           Random.Range(-(halfBounds.z), halfBounds.z));
    }

    /// <summary>
    /// Checks if A Position is In The Flocking Bounds.
    /// </summary>
    /// <param name="_positionToCheck"></param>
    /// <returns>Returns True or False based On if The Position is In The Bounds.</returns>
    public static bool CheckInBounds(Vector3 _positionToCheck)
    {
        return boundsCollider.bounds.Contains(_positionToCheck);
    }
    #endregion

    #region  Visualization Functions
    /// <summary>
    /// Enables The Visualizer GameObject Based on The Parameter.
    /// </summary>
    /// <param name="isEnabled">The Desired GameObject Visualizer Visability.</param>
    private void EnableVisualizer(bool isEnabled)
    {
        targetVisualizer.SetActive(isEnabled);
    }

    /// <summary>
    /// Moves The Target Visualizer To The Current Target Position.
    /// </summary>
    private void MoveTargetVisualizer()
    {
        targetVisualizer.transform.position = targetPosition;
    }
    #endregion

}