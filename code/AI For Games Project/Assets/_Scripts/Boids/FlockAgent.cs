using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    [Header("Bird Attributes")]
    /// <summary>
    /// The Physics LayerMask The Boids Are Calculated On.
    /// </summary>
    [SerializeField] private LayerMask boidsLayerMask = LayerMask.NameToLayer("Boids");

    /// <summary>
    /// The Movement Speed of The Boid Agent.
    /// </summary>
    private float movementSpeed = 1f;

    /// <summary>
    /// The Minimum Movement Speed of The Boid Agent.
    /// </summary>
    public float minMovementSpeed = 1f;

    /// <summary>
    /// The Maximum Movement Speed of The Boid Agent.
    /// </summary>
    public float maxMovementSpeed = 5f;

    /// <summary>
    /// The Rotation Speed of The Boid Agent.
    /// </summary>
    public float rotationSpeed = 3f;

    /// <summary>
    /// If The Boid Is Turning Back Towards The Target or Not.
    /// </summary>
    public bool turning = true;

    [Space]

    /// <summary>
    /// The Range At Which To Detect Nearby Boid Agents.
    /// </summary>
    public float neighbourRange = 10f;

    [Header("Steering Behaviours")]
    /// <summary>
    /// The Range At Which To Seperate From Neighbouring Boid Agents.
    /// </summary>
    public float seperationRange = 6f;

    #region Unity Functions
    /// <summary>
    /// Unitys Start Function.
    /// </summary>
    private void Start()
    {
        movementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
    }

    /// <summary>
    /// Unitys Update Function.
    /// </summary>
    private void Update()
    {
        if (Vector3.Distance(transform.position, FlockManager.instance.TargetPosition) >= 150) turning = true;

        if (turning)
        {
            Vector3 direction = FlockManager.instance.GetPositionInBounds() - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction),
                                                  rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, FlockManager.instance.TargetPosition) <= 30f) turning = false;
        }
        else if (Random.Range(0, 5) < 1)
        {
            Vector3 boidCohesion = transform.position;            
            Vector3 boidSeperation = Vector3.zero;
            Vector3 boidAlignment;

            int neighbourAmount = 0;
            float neighbourDistance;
            float neighbourSpeed = 0.5f;

            foreach (Collider bird in Physics.OverlapSphere(transform.position, neighbourRange, boidsLayerMask, QueryTriggerInteraction.Collide))
            {
                if (bird.TryGetComponent<FlockAgent>(out FlockAgent neighbourAgent) && bird.gameObject != this.gameObject)
                {
                    neighbourDistance = Vector3.Distance(bird.transform.position, this.transform.position);
                    if (neighbourDistance <= neighbourRange)
                    {
                        neighbourAmount++;
                        boidCohesion += bird.transform.position;

                        if (neighbourDistance < seperationRange)
                        {
                            boidSeperation = Seperation(boidSeperation, bird.transform);                        
                        }
                    
                        neighbourSpeed += neighbourAgent.movementSpeed;
                    }
                }
            }

            if (neighbourAmount > 0)
            {
                boidCohesion = Cohesion(boidCohesion, neighbourAmount);
                movementSpeed = (neighbourSpeed / neighbourAmount) + Random.Range(-0.1f, 0.1f);
                if (movementSpeed > maxMovementSpeed) movementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);

                boidAlignment = Alignment(boidCohesion, boidSeperation);

                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                      Quaternion.LookRotation(boidAlignment),
                                                      rotationSpeed * Time.deltaTime);
            }
        }
        transform.Translate(0, 0, Time.deltaTime * movementSpeed);
    }
    #endregion

    #region Steering Behaviors

    /// <summary>
    /// Calculates The Cohesion Vector of The Boids Movement.
    /// </summary>
    /// <param name="_currentCohesionVector">The Current Cohesion Vector.</param>
    /// <param name="_neighbourAmount">The Amount of Neighbour Agents Detected.</param>
    /// <returns></returns>
    private Vector3 Cohesion(Vector3 _currentCohesionVector, int _neighbourAmount)
    {
        return _currentCohesionVector / _neighbourAmount + (FlockManager.instance.TargetPosition - transform.position);
    }

    /// <summary>
    /// Calculates The Seperation Vector of The Boids Movement.
    /// </summary>
    /// <param name="_currentSeperationVector">The Current Seperation Vector.</param>
    /// <param name="_neighbourBird">The Transform of a Neighbouring Bird.</param>
    /// <returns></returns>
    private Vector3 Seperation(Vector3 _currentSeperationVector, Transform _neighbourBird)
    {
        return _currentSeperationVector + (transform.position - _neighbourBird.position);
    }

    /// <summary>
    /// Calculates The Alignment Vector of The Boids Movement.
    /// </summary>
    /// <param name="_cohesionVector">The Current Cohesion Vector.</param>
    /// <param name="_seperationVector">The Current Seperation Vector.</param>
    /// <returns></returns>
    private Vector3 Alignment(Vector3 _cohesionVector, Vector3 _seperationVector)
    {
        return (_cohesionVector + _seperationVector) - transform.position;
    }

    #endregion
}
