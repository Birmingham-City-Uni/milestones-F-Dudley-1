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
    public float rotationSpeed = 10f;
    public float boostMultiplier = 10f;

    private float xEulerRotation, yEulerRotation;

    private Transform cameraTransform;
    private Transform cameraContainer;

    [Header("Interaction Variables")]
    public float interactionRange = 100f;

    [Header("Character Variables")]
    public bool controllingCharacter;

    private CharacterMovement characterScript;

    #region Unity Functions
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
                CheckCameraRay();
                UpdateCameraTransform();
                break;

            case true:
                CharacterControl();
                break;
        }

        UserInput();
    }
    #endregion

    #region User Input
    private void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RemoveParent();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            // Toggle Debug Settings.
        }
    }

    private float GetMouseY() => Input.GetAxis("Mouse Y") * rotationSpeed;
    private float GetMouseX() => Input.GetAxis("Mouse X") * rotationSpeed;

    private float GetMovementX() => Input.GetAxis("Horizontal") * Time.deltaTime;
    private float GetMovementY() => Input.GetAxis("Vertical") * Time.deltaTime;
    #endregion

    #region Camera Functions
    private void SetControl(CharacterMovement controller)
    {
        if (controller != null)
        {
            characterScript = controller;
            controllingCharacter = true;
            characterScript.currentlyControlled = true;
        }
        else
        {
            controllingCharacter = false;
            if (characterScript)
            {
                characterScript.currentlyControlled = false;
                characterScript = null;
            }
        }
    }

    private void UpdateCameraTransform()
    {
        float speed = movementSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) speed *= boostMultiplier;

        // Rotation

        float newRotationX = transform.localEulerAngles.x - GetMouseY();
        float newRotationY = transform.localEulerAngles.y + GetMouseX();

        transform.localRotation = Quaternion.Euler(newRotationX, newRotationY, 0.0f);

        // Movement
        transform.position += transform.forward * (GetMovementY() * speed);
        transform.position += transform.right * (GetMovementX() * speed);
    }

    private void CheckCameraRay()
    {
        RaycastHit hit;

        bool castCollide = Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, LayerMask.GetMask("Characters"));
        if (castCollide)
        {
            Debug.Log("Casting on Character");
            CharacterMovement controller = hit.collider.transform.root.GetComponent<CharacterMovement>();

            if (Input.GetKeyDown(KeyCode.F))
            {
                transform.SetParent(controller.cameraPosition);
                LeanTween.move(this.gameObject, controller.cameraPosition, 1.0f).setEaseInCubic()
                .setOnStart(() =>{
                    LeanTween.rotate(this.gameObject, controller.cameraPosition.forward, 1.0f).setEaseInCubic();
                })
                .setOnComplete(() =>{
                    if (controller.userControllable)
                    {
                        SetControl(controller);
                    }
                });

            }
        }

        Debug.DrawRay(transform.position, transform.forward * interactionRange, castCollide ? Color.red : Color.green, 0.0f);
    }

    private void RemoveParent()
    {
        if (transform.parent)
        {
            transform.SetParent(null, true);

            SetControl(null);
        }
    }
    #endregion

    #region Character Control Functions
    private void CharacterControl()
    {
        // Rotation
        float newRotationX = transform.localEulerAngles.x - GetMouseY();

        transform.localRotation = Quaternion.Euler(newRotationX, 0.0f, 0.0f);
        characterScript.UpdateCharacterRotation(GetMouseX());

        // Position
        characterScript.UpdateCharacterPosition(GetMovementY(), GetMovementX());
    }
    #endregion
}
