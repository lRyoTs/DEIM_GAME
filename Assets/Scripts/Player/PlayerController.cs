using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput),typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float softRotation = 0.5f;

    [SerializeField]
    private Transform cameraTransform;
    
    private CharacterController _characterController;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
   
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }


    void Update()
    {
        HandleMovement();
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
        _characterController.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        _characterController.Move(playerVelocity * Time.deltaTime);

       
        //Rotate towards camera
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0,targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, softRotation * Time.deltaTime);        
    }
}
