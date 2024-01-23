using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    //private CharacterController _characterController;
    [SerializeField] private Transform cam;
    PlayerController playerController;

    public float speed = 3f;
    
    public float turnSmoothTime = 0.15f;
    private float turnSmoothVelocity;

    private void Awake()
    {
        //_characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = horizontalInput * cam.transform.right + verticalInput* cam.transform.forward;
        
        transform.Translate (direction.normalized * speed * Time.deltaTime,Space.World);
        
    }

    private void HandleMovement() {
        /*
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 direction = inputVector.x * cam.transform.right + inputVector.z * cam.transform.forward;
        */
    }
}
