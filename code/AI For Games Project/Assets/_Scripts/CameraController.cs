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

    /// <summary>
    /// The Multiplier of The Characters Boost Movement.
    /// </summary>
    public float boostMultiplier = 10f;

    /// <summary>
    /// The Characters X and Y Euler Rotation.
    /// </summary>
    private float xEulerRotation, yEulerRotation;

    [Header("Interaction Variables")]
    /// <summary>
    /// The Interaction Range The Camera Can Interact An Attach From.
    /// </summary>
    public float interactionRange = 100f;

    [Header("Character Variables")]
    /// <summary>
    /// The LayerMask of Characters The Camera Can Attach To.
    /// </summary>
    public LayerMask characterLayerMask;

    /// <summary>
    /// Wether The Camera is Controlling an Attached Character.
    /// </summary>
    public bool controllingCharacter;

    /// <summary>
    /// The Attached Characters Movement Script. 
    /// </summary>
    private CharacterMovement characterScript;

    #region Unity Functions
    /// <summary>
    /// Unitys Start Functon.
    /// </summary>
    private void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Unitys Update Function.
    /// </summary>
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
                if (characterScript.userControllable)
                {
                    CharacterInput();
                    CharacterControl();                    
                }
                break;
        }

        UserInput();
    }
    #endregion

    #region User Input

    /// <summary>
    /// The User Input That Can Be Performed Towards the Camera.
    /// </summary>
    private void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RemoveParent();
        }
    }

    /// <summary>
    /// Gets The Mouses Current Y Value.
    /// </summary>
    /// <returns>A Float of The Mouses Y Value and Rotation Speed.</returns>
    private float GetMouseY() => Input.GetAxis("Mouse Y") * rotationSpeed;

    /// <summary>
    /// Gets The Mouses Current X Value.
    /// </summary>
    /// <returns>A Float of The Mouses X Value and Rotation Speed.</returns>
    private float GetMouseX() => Input.GetAxis("Mouse X") * rotationSpeed;

    /// <summary>
    /// Gets The Current Horizontal Movement Axis.
    /// </summary>
    /// <returns>Returns a Float of The Horizontal Axis Minimised For Current FPS.</returns>
    private float GetMovementX() => Input.GetAxis("Horizontal") * Time.deltaTime;

    /// <summary>
    /// Gets The Current Vertical Movement Axis.
    /// </summary>
    /// <returns>Returns a Float of The Vertical Axis Minimised For Current FPS.</returns>
    private float GetMovementY() => Input.GetAxis("Vertical") * Time.deltaTime;
    #endregion

    #region Camera Functions
    /// <summary>
    /// Sets The Control Functions of the Camera on Characters, if they are Controllable.
    /// </summary>
    /// <param name="controller">The Character Controller Script.</param>
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

    /// <summary>
    /// Updates The Cameras Transform Based On Input Movement.
    /// </summary>
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

    /// <summary>
    /// Casts a Ray To Check if a Camera Collides With a Character.
    /// </summary>
    private void CheckCameraRay()
    {
        RaycastHit hit;

        bool castCollide = Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, characterLayerMask);
        if (castCollide)
        {
            //Debug.Log("Casting on Character");
            CharacterMovement controller = hit.collider.transform.root.GetComponent<CharacterMovement>();

            if (Input.GetKeyDown(KeyCode.F))
            {
                transform.SetParent(controller.cameraPosition);
                LeanTween.move(this.gameObject, controller.cameraPosition, 1.0f).setEaseInCubic()
                .setOnStart(() =>
                {
                    LeanTween.rotate(this.gameObject, controller.cameraPosition.forward, 1.0f).setEaseInCubic();
                })
                .setOnComplete(() =>
                {
                    SetControl(controller);
                });
            }
        }

        Debug.DrawRay(transform.position, transform.forward * interactionRange, castCollide ? Color.red : Color.green, 0.0f);
    }

    /// <summary>
    /// Removes the Cameras Current Parent if Attached To A Character.
    /// </summary>
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

    private void CharacterInput()
    {   
        if (Input.GetButtonDown("Jump"))
        {
            characterScript.CharacterJump();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            characterScript.CharacterShout();
        }
    }
    #endregion
}
