using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput),typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    public enum PlayerState {
        Normal,
        Tired,
        Dead
    }

    public PlayerState _playerState;

    [Header("References")]
    private PlayerInput _playerInput;
    private CharacterController _characterController;
    private PlayerControls _input;
    private Animator _animator;
    private PlayerStats _playerStats;
    private LevelSystem _levelSystem;

    [SerializeField]
    private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float softRotation = 0.5f;
    private Vector3 verticalVelocity;
    private bool groundedPlayer;
    private bool isWalking;

    [Header("Dash")]
    private bool canDash = true;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
   
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private const float _threshold = 0.01f;
    #endregion

    private void Awake()
    {
        _playerState = PlayerState.Normal;
        //Get References
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<PlayerControls>();
        _animator = GetComponent<Animator>();
        _playerStats = GetComponent<PlayerStats>();
        _levelSystem = GetComponent<LevelSystem>();

        DesactivateCharacterController(); //Desactivate characterController to move player position
                                          //Because Character controller overwrites player.transform.position to its previous position

        canDash = true;
        EventManager.AddHandler(EventManager.EVENT.OnPause, UnlockCursor);
        EventManager.AddHandler(EventManager.EVENT.OnResume, LockCursor);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(EventManager.EVENT.OnPause, UnlockCursor);
        EventManager.RemoveHandler(EventManager.EVENT.OnResume, LockCursor);
    }

    void Update()
    {
        switch (_playerState) {
            case PlayerState.Normal:
                Jump();
                HandleMovement();
                Shoot();
                break;
            case PlayerState.Tired:
                HandleMovement();
                break;
            case PlayerState.Dead:
                break;
        }
    }

    private void LateUpdate()
    {
        switch (_playerState)
        {
            case PlayerState.Normal:
                Look();
                break;
            case PlayerState.Tired:
                Look();
                break;
            case PlayerState.Dead:
                break;
        }

        //Player Animations
        _animator.SetBool("Walk",isWalking);
    }

    private Vector3 GetMoveDirection()
    {
        Vector3 direction = new Vector3(_input.Move.x, 0, _input.Move.y).normalized;
        return direction;
    }

    private void HandleMovement() {

        float targetSpeed;

        if(_input.Move != Vector2.zero)
        {
            targetSpeed = _playerState != PlayerState.Tired ? playerSpeed : playerSpeed/2 ;
            isWalking = true ;
        }
        else
        {
            isWalking= false ;
            targetSpeed = 0 ;
        }
        

        isWalking = _input.Move != Vector2.zero ? true : false;

        Vector3 direction = GetMoveDirection();
        
        //direction = direction.x * cameraTransform.right.normalized + direction.z * cameraTransform.forward.normalized;

        //Rotate towards camera
        float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0,targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, softRotation * Time.deltaTime);

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
        
        //If dash input pressed and player not tired
        if(_input.Dash && canDash && _playerState != PlayerState.Tired)
        {
            //Do dash
            StartCoroutine(Dash(targetDirection));
        }
        else
        {
            //Move player
            _characterController.Move((targetDirection.normalized * targetSpeed * Time.deltaTime) + verticalVelocity * Time.deltaTime);
        }
       
    }

    private void Jump() {
        groundedPlayer = _characterController.isGrounded;
        verticalVelocity.y += gravityValue * Time.deltaTime;
        if (groundedPlayer)
        {
            //Stop velocity for dropping infinitely when grounded 
            if (verticalVelocity.y < 0.0f)
            {
                verticalVelocity.y = -2f;
            }

            //Jump
            if (_input.Jump)
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityValue); //velocity to reach desired height
                _animator.SetTrigger("Jump");
            }
        }
        else {
            _input.Jump = false;
        }
    }

    /// <summary>
    /// Move virtual camera to aling to the desired camera angle
    /// </summary>
    private void Look() {
        // if there is an input and camera position is not fixed
        if (_input.Look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = Time.deltaTime;

            _cinemachineTargetYaw += _input.Look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.Look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    /// <summary>
    /// Limits camera angle
    /// </summary>
    /// <param name="lfAngle">CurrentAngle</param>
    /// <param name="lfMin">MinAngle</param>
    /// <param name="lfMax">MaxAngle</param>
    /// <returns></returns>
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    /// <summary>
    /// Coroutine that start Dash
    /// </summary>
    /// <param name="targetDirection">Desired direction</param>
    /// <returns></returns>
    private IEnumerator Dash(Vector3 targetDirection)
    {
        canDash = false;
        _input.Dash = false;
        float startTime = Time.time;
        while (Time.time < startTime + dashTime) {
            _characterController.Move(targetDirection.normalized * dashSpeed * Time.deltaTime);
            yield return null;
        }
        canDash = true;
    }

    private void Shoot() {
        throw new NotImplementedException();
    }

    public void SetPlayerState(PlayerState playerState)
    {
        _playerState = playerState;
    }

    public void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
    }

    public void DesactivateCharacterController()
    {
        _characterController.enabled = false;
    }

    public void ActivateCharacterController() {
        _characterController.enabled = true;
    }
}
