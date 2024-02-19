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
    private PlayerInput playerInput;
    private CharacterController _characterController;
    [SerializeField]
    private Transform cameraTransform;
    public Transform ProjectileSpawnPosition => throw new System.NotImplementedException();
    public GameObject BulletPrefab => throw new System.NotImplementedException();

    [Header("Movement")]
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float softRotation = 0.5f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
   
    [Header("Inputs")]
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;

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
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        lookAction = playerInput.actions["Look"];
    }


    void Update()
    {
        switch (_playerState) {
            case PlayerState.OnField:
                HandleMovement();
                break;
            case PlayerState.OnBattle:
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

        float targetSpeed = moveAction.ReadValue<Vector2>() != Vector2.zero ? playerSpeed : 0 ;

        groundedPlayer = _characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Move by InputAction and looking and the camera direction
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        //_characterController.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
       
        //Rotate towards camera
        float targetAngle = Mathf.Atan2(move.x,move.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0,targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, softRotation * Time.deltaTime);

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
        //Move player
        _characterController.Move((targetDirection.normalized * targetSpeed * Time.deltaTime) + playerVelocity);
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
        if (lookAction.ReadValue<Vector2>().sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = /*IsCurrentDeviceMouse ? 1.0f :*/ Time.deltaTime;

            _cinemachineTargetYaw += lookAction.ReadValue<Vector2>().x * deltaTimeMultiplier;
            _cinemachineTargetPitch += lookAction.ReadValue<Vector2>().y * deltaTimeMultiplier;
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
        if (collision.gameObject.CompareTag("Enemy")) {
            playerInput.SwitchCurrentActionMap("PlayerOnBattle");
            _playerState = PlayerState.OnBattle;
        }
          
   }
}
