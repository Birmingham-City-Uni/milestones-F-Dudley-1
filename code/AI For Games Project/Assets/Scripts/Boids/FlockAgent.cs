using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    [Header("Bird Attributes")]
    public float movementSpeed = 1f;
    public float maxMovementSpeed = 5f;
    public float minMovementSpeed = 1f;

    public float rotationSpeed = 3f;
    public bool turning = true;

    [Space]

    public float neighbourRange = 10f;

    [Header("Steering Behaviours")]
    public Vector3 lastPlayerPos;
    public float seperationRange = 6f;


    [Header("Enviroment References")]
    [SerializeField] private Transform player;

    public void SetPlayer(Transform _player) => player = _player;

    #region Unity Functions
    private void Start()
    {
        movementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
        lastPlayerPos = player.position;
    }

    private void Update()
    {
        if (!FlockManager.boundsCollider.bounds.Contains(transform.position))
        {
            turning = true;
        }
        else if (transform.position.y <= 10f)
        {
            turning = true;
        }
        else turning = false;
        
        if (turning)
        {
            Vector3 direction = (player.position + FlockManager.instance.GetPositionInBounds()) - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction),
                                                  rotationSpeed * Time.deltaTime);
        }

        if (Random.Range(0, 5) < 1)
        {
            Vector3 boidCohesion = transform.position;            
            Vector3 boidSeperation = lastPlayerPos;
            Vector3 boidAlignment;

            int neighbourAmount = 0;
            float neighbourDistance;
            float neighbourSpeed = 0.5f;

            foreach (GameObject bird in FlockManager.instance.birds)
            {
                if (bird != this.gameObject)
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

                        neighbourSpeed += bird.GetComponent<FlockAgent>().movementSpeed;
                    }                    
                }
            }

            if (neighbourAmount >= 0)
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

    private Vector3 Cohesion(Vector3 _currentCohesionVector, int _neighbourAmount)
    {
        return _currentCohesionVector / _neighbourAmount + (FlockManager.instance.targetPosition - transform.position);
    }

    private Vector3 Seperation(Vector3 _currentSeperationVector, Transform _neighbourBird)
    {
        return _currentSeperationVector + (transform.position - _neighbourBird.position);
    }

    private Vector3 Alignment(Vector3 _cohesionVector, Vector3 _seperationVector)
    {
        return (_cohesionVector + _seperationVector) - transform.position;
    }

    #endregion
}
