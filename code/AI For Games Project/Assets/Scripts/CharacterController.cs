using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [Header("Main Variables")]
    public bool userControllable = false;
    public bool currentlyControlled = false;

    [Header("Physics Variables")]
    public float movementSpeed = 1f;
    public float sprintMultiplier;
    public Transform groundCheck;

    private Transform playerTransform;

#region Unity Functions

    void Start()
    {
        playerTransform = GetComponent<Transform>();
        if(userControllable) {
            currentlyControlled = false;
        }
    }

    void Update()
    {

    }

#endregion

    private float GetMovementX() => Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
    private float GetMovementY() => Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

    public void UpdateCharacterPosition(Vector3 newPosition) => playerTransform.position = newPosition;    
    public void UpdateCharacterRotation(Vector3 newRotation) => playerTransform.Rotate(newRotation);

    private void UserControl() {
        float horizontal = GetMovementX();
        float vertical = GetMovementY();
    }

    private void ComputerControl() {

    }
}
