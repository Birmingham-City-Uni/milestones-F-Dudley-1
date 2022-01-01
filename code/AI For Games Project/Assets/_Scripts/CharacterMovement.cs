using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Main Variables")]
    public bool userControllable = false;
    public bool currentlyControlled = false;

    [Header("Physics Variables")]
    public float movementSpeed = 1f;
    public float jumpHeight = 1f;
    public float sprintMultiplier;

    public float shoutDistance = 7f;

    [Space]

    public LayerMask groundMask;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    [SerializeField] private float velocity = 0.0f;
    private Transform playerTransform;
    private CharacterController characterController;

    [Header("Camera Variables")]
    public Transform cameraPosition;

    private AudioSource walkingAudio;
    private AudioSource shoutingAudio;

    #region Unity Functions    
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();

        walkingAudio = GetComponent<AudioSource>();
        shoutingAudio = cameraPosition.GetComponent<AudioSource>();
        shoutingAudio.maxDistance = shoutDistance;            

        currentlyControlled = false;
    }

    void Update()
    {
        isGrounded = Physics.CheckBox(groundCheck.position, new Vector3(groundDistance, 0.2f, groundDistance) / 2.0f, transform.rotation, groundMask);
    }

    void FixedUpdate()
    {
        UpdateCharacterVelocity();
    }

    void OnDrawGizmos()
    {
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

    public void UpdateCharacterPosition(float _vertical, float _horizontal)
    {
        if ((!isGrounded || (_vertical == 0 && _horizontal == 0)) && walkingAudio.isPlaying) walkingAudio.Stop();
        else if (isGrounded && !walkingAudio.isPlaying && (_vertical != 0 || _horizontal != 0)) walkingAudio.Play();

        Vector3 movement = (transform.forward * _vertical) + (transform.right * _horizontal);

        characterController.Move(movement * movementSpeed);
    }

    public void UpdateCharacterPosition(Vector3 _targetPos)
    {
        if (isGrounded && !walkingAudio.isPlaying) walkingAudio.Play();

        Vector3 movement = _targetPos - transform.position;
        characterController.Move((movement).normalized * movementSpeed * Time.deltaTime);
    }

    public void UpdateCharacterRotation(float _mouseX)
    {
        float newRotationY = transform.localEulerAngles.y + _mouseX;
        transform.localRotation = Quaternion.Euler(0.0f, newRotationY, 0.0f);
    }

    public void UpdateCharacterVelocity()
    {
        velocity += (-20f * Mathf.Pow(Time.deltaTime, 2.0f));
        if (isGrounded && velocity < 0) velocity = 0;
        characterController.Move(new Vector3(0.0f, velocity, 0.0f));
    }

    public void CharacterJump()
    {
        if (isGrounded && userControllable) velocity = Mathf.Sqrt(jumpHeight * -2 * -20);
    }

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

    public void StopCharacterAudio()
    {
        if (walkingAudio.isPlaying)
        {
            walkingAudio.Stop();
        }
    }
}
