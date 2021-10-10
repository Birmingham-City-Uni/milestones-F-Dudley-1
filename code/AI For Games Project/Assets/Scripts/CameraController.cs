using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Physics Variables")]
    /// <summary>
    /// The Cameras Movement Speed.
    /// </summary>
    public float movementSpeed = 10f;
    /// <summary>
    /// The Rotation of the Camera or Selected Character.
    /// </summary>
    public float rotationSpeed = 100f;
    public float boostMultiplier = 10f;

    private float xEulerRotation, yEulerRotation;

    private Transform cameraTransform;
    private Transform cameraContainer;
    [Header("Character Variables")]
    public bool controllingCharacter;

    private CharacterController characterScript;

    private void Start()
    {
        cameraTransform = GetComponent<Transform>();
        cameraContainer = transform.parent;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        switch (controllingCharacter)
        {
            default:
            case false:
                UserInput();
                UpdateCameraTransform();
                break;

            case true:
                Debug.Log("Controlling Character");
                break;
        }

        UserInput();
    }

#region User Input

    private void UserInput() {

        if(Input.GetKeyDown(KeyCode.P)) {
            // Toggle Between Character and Cam.
        }

        if(Input.GetKeyDown(KeyCode.L)) {
            // Toggle Debug Settings.
        }
    }

    private float GetMouseY() => Input.GetAxis("Mouse Y") * rotationSpeed;
    private float GetMouseX() => Input.GetAxis("Mouse X") * rotationSpeed;

    private float GetMovementX() {
        float speed = movementSpeed;
        if(Input.GetKey(KeyCode.LeftShift)) speed *= boostMultiplier;
        float horizontal =Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;

        return horizontal;
    }

    private float GetMovementY() {
        float speed = movementSpeed;
        if(Input.GetKey(KeyCode.LeftShift)) speed *= boostMultiplier;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        return vertical;
    }

#endregion

    private void UpdateCameraTransform() {

        // Rotation
        float newRotationX = transform.localEulerAngles.x - GetMouseY();
        float newRotationY = transform.localEulerAngles.y + GetMouseX();

        transform.localRotation = Quaternion.Euler(newRotationX, newRotationY, 0.0f);

        // Movement
        transform.position += transform.forward * GetMovementY();
        transform.position += transform.right * GetMovementX();
    }
}
