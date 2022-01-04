using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Main Variables")]

    /// <summary>
    /// Specifies if a Player Can Control The Character.
    /// </summary>
    public bool userControllable = false;

    /// <summary>
    /// Specifies if The Player is Currently Controlling the Character.
    /// </summary>
    public bool currentlyControlled = false;

    [Header("Physics Variables")]

    /// <summary>
    /// The Movement Speed of the Character.
    /// </summary>
    public float movementSpeed = 1f;

    /// <summary>
    /// The Rotation Speed of The Character.
    /// </summary>
    public float rotationSpeed = 1f;

    /// <summary>
    /// The Jump Height of The Character.
    /// </summary>
    public float jumpHeight = 1f;

    /// <summary>
    /// The Sprint Multiplier of The Character.
    /// </summary>
    public float sprintMultiplier;

    /// <summary>
    /// The Distance at Which The Characters Shout Can Be Heard.
    /// </summary>
    public float shoutDistance = 7f;

    [Space]

    /// <summary>
    /// The LayerMask of The Ground.
    /// </summary>
    public LayerMask groundMask;

    /// <summary>
    /// Wether The Character is Grounded or Not.
    /// </summary>
    public bool isGrounded;

    /// <summary>
    /// The Location Where The Ground Check is Performed.
    /// </summary>
    public Transform groundCheck;

    /// <summary>
    /// The Distance The Ground Check is Performed.
    /// </summary>
    public float groundDistance = 0.4f;

    /// <summary>
    /// The Current Y Velocity of the Character.
    /// </summary>
    [SerializeField] private float velocity = 0.0f;

    /// <summary>
    /// The Characters CharacterController Component.
    /// </summary>
    private CharacterController characterController;

    [Header("Player Variables")]

    /// <summary>
    /// The Position The Attached Camera Goes.
    /// </summary>
    public Transform cameraPosition;

    /// <summary>
    /// The Characters Animations.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// The Walking Audio of The Character.
    /// </summary>
    private AudioSource walkingAudio;

    /// <summary>
    /// The Shouting Audio of The Player.
    /// </summary>
    private AudioSource shoutingAudio;

    #region Unity Functions
    /// <summary>
    /// Unitys Start Function.
    /// </summary>
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        walkingAudio = GetComponent<AudioSource>();
        shoutingAudio = cameraPosition.GetComponent<AudioSource>();
        shoutingAudio.maxDistance = shoutDistance;

        currentlyControlled = false;
    }

    /// <summary>
    /// Unitys Update Function.
    /// </summary>
    private void Update()
    {
        isGrounded = Physics.CheckBox(groundCheck.position, new Vector3(groundDistance, 0.2f, groundDistance) / 2.0f, transform.rotation, groundMask);
    }

    /// <summary>
    /// Unitys FixedUpdate Function.
    /// </summary>
    private void FixedUpdate()
    {
        UpdateCharacterVelocity();
    }

    /// <summary>
    /// Unitys OnDrawGizmos Function.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(cameraPosition.position, transform.forward * 2f);

        // Shout Visual
        if (userControllable)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(cameraPosition.position, shoutDistance);
        }

        // Ground Check
        if (isGrounded) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawCube(groundCheck.position, new Vector3(groundDistance, 0.2f, groundDistance) / 2.0f);
    }

    #endregion

    /// <summary>
    /// Updates The Characters Potion Depending on the Vertical and Horizontal Values.
    /// </summary>
    /// <param name="_vertical">The Vertiacl Axis Values.</param>
    /// <param name="_horizontal">The Horizontal Axis Values.</param>
    public void UpdateCharacterPosition(float _vertical, float _horizontal)
    {
        if ((!isGrounded || (_vertical == 0 && _horizontal == 0)) && walkingAudio.isPlaying)
        {
           walkingAudio.Stop();
           animator.SetBool("isMoving", false);
        }
        else if (isGrounded && !walkingAudio.isPlaying && (_vertical != 0 || _horizontal != 0))
        {
            walkingAudio.Play();
            animator.SetBool("isMoving", true);
        }

        Vector3 movement = (transform.forward * _vertical) + (transform.right * _horizontal);

        characterController.Move(movement * movementSpeed);
    }

    /// <summary>
    /// Updates The Characters Postion, Towards a Target Postion.
    /// </summary>
    /// <param name="_targetPos">The Target Position To Move Towards.</param>
    public void UpdateCharacterPosition(Vector3 _targetPos)
    {
        if (isGrounded && !walkingAudio.isPlaying)
        {
            walkingAudio.Play();
            animator.SetBool("isMoving", true);
        }

        Vector3 movementDirection = _targetPos - transform.position;
        characterController.Move((movementDirection).normalized * movementSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Updates The Characters Euler Y Rotation.
    /// </summary>
    /// <param name="_mouseX">The Mouse X Axis.</param>
    public void UpdateCharacterRotation(float _mouseX)
    {
        float newRotationY = transform.localEulerAngles.y + _mouseX;
        transform.localRotation = Quaternion.Euler(0.0f, newRotationY, 0.0f);
    }

    /// <summary>
    /// Updates The Characters Rotation Towards A Target Postion.
    /// </summary>
    /// <param name="_targetPos">The Target Position To Rotate Towards.</param>
    public void UpdateCharacterRotation(Vector3 _targetPos)
    {
        Vector3 movementDirection = _targetPos - transform.position;
        float angle = Vector3.Angle(transform.forward, _targetPos - transform.position);

        if (Vector3.Cross(transform.forward, movementDirection).y < 0.0f) angle *= -1;
        transform.Rotate(0, angle * Time.deltaTime, 0);
    }

    /// <summary>
    /// Updates The Characters Y Velocity, To Simulate Gravity.
    /// </summary>
    public void UpdateCharacterVelocity()
    {
        velocity += (-20f * Mathf.Pow(Time.deltaTime, 2.0f));
        if (isGrounded && velocity < 0) velocity = -5;
        characterController.Move(new Vector3(0.0f, velocity, 0.0f));
    }

    /// <summary>
    /// Makes The Character Perform a Jump if Possible.
    /// </summary>
    public void CharacterJump()
    {
        if (isGrounded && userControllable) velocity = Mathf.Sqrt(jumpHeight * -2 * -20);
    }

    /// <summary>
    /// Makes The Character Shout.
    /// </summary>
    public void CharacterShout()
    {
        if (userControllable)
        {
            if (!shoutingAudio.isPlaying) shoutingAudio.Play();

            int i = 0; // Debug Variable
            foreach (Collider characterCollider in Physics.OverlapSphere(cameraPosition.position, shoutDistance, LayerMask.GetMask("Characters"), QueryTriggerInteraction.Ignore))
            {
                i++;
                CharacterInfo characterInfo = characterCollider.gameObject.GetComponent<CharacterInfo>();

                if (characterInfo != null)
                {
                    characterInfo.AlertedLocation = transform.position;
                }
            }

            Debug.Log(string.Format("Shout Hit {0} Characters", i));
        }
    }

    /// <summary>
    /// Sets The Characters Animation Parameter.
    /// </summary>
    /// <param name="animationParameter">The Animators Parameter Name.</param>
    /// <param name="animationValue">The Desired Value of The Parameter.</param>
    public void SetCharacterAnimations(string animationParameter, bool animationValue)
    {
        animator.SetBool(animationParameter, animationValue);
    }

    /// <summary>
    /// Stops The Characters Walking Audio.
    /// </summary>
    public void StopCharacterAudio()
    {
        if (walkingAudio.isPlaying)
        {
            walkingAudio.Stop();
        }
    }
}
