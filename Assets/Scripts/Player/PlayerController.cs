using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    #endregion

    private void Awake()
    {
        _playerState = PlayerState.OnField;
        _characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
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


    private void HandleMovement() {

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
        _characterController.Move((move + playerVelocity)* playerSpeed * Time.deltaTime);

       
        //Rotate towards camera
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0,targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, softRotation * Time.deltaTime);        
    }


    private void Dodge()
    {
        throw new NotImplementedException();
    }

    private void Shoot() {
        throw new System.NotImplementedException();
    }

    private void Look() {
        //Vector2 targetMouseDelta = playerInput.actions["Look"].ReadValue()*Time.smoothDeltaTime;
    }

   private void OnCollisionEnter(Collision collision)
   {
        if (collision.gameObject.CompareTag("Enemy")) {
            playerInput.SwitchCurrentActionMap("PlayerOnBattle");
            _playerState = PlayerState.OnBattle;
        }
          
   }
}
