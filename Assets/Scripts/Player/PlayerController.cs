using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput),typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IProjectile
{
    #region Variables
    public enum PlayerState {
        OnField,
        OnBattle,
        Death
    }

    private PlayerState _playerState;

    [Header("References")]
    private PlayerInput _playerInput;
    private CharacterController _characterController;
    private PlayerControls _input;

    [SerializeField]
    private Transform cameraTransform;
    public Transform ProjectileSpawnPosition => throw new System.NotImplementedException();
    public GameObject BulletPrefab => throw new System.NotImplementedException();

    [Header("Movement")]
    private Vector3 startPosition;
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
        _playerState = PlayerState.OnField;
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<PlayerControls>();

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
            case PlayerState.OnField:
                Jump();
                HandleMovement();
                break;
            case PlayerState.OnBattle:
                Jump();
                Shoot();
                Dodge();
                break;
            case PlayerState.Death:
                break;
        }
    }

    private void LateUpdate()
    {
        switch (_playerState)
        {
            case PlayerState.OnField:
                Look();
                break;
            case PlayerState.OnBattle:
                Look();
                break;
            case PlayerState.Death:
                break;
        }
    }

    private void HandleMovement() {

        float targetSpeed = _input.Move != Vector2.zero ? playerSpeed : 0 ;

        Vector3 move = new Vector3(_input.Move.x, 0, _input.Move.y);
        
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;

        //Rotate towards camera
        float targetAngle = Mathf.Atan2(move.x,move.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0,targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, softRotation * Time.deltaTime);

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
        //Move player
        _characterController.Move((targetDirection.normalized * targetSpeed * Time.deltaTime) + verticalVelocity * Time.deltaTime);
    }

    private void Jump() {
        groundedPlayer = _characterController.isGrounded;
        Debug.Log(groundedPlayer);
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
            }
        }
        else {
            _input.Jump = false;
        }

        if (verticalVelocity.y < 53)
        {
            verticalVelocity.y += gravityValue * Time.deltaTime;
        }
    }

    private void Dodge()
    {
        throw new NotImplementedException();
    }

    private void Shoot() {
        throw new System.NotImplementedException();
    }

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
        //Vector2 targetMouseDelta = playerInput.actions["Look"].ReadValue()*Time.smoothDeltaTime;
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Enter in combat
        if (collision.gameObject.CompareTag("Enemy")) {
            _playerInput.SwitchCurrentActionMap("PlayerOnBattle");
            _playerState = PlayerState.OnBattle;
        }
    }

    public void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
    }
}
