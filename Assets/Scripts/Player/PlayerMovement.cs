using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    //private CharacterController _characterController;
    [SerializeField] private Transform cam;

    public float speed = 3f;
    
    public float turnSmoothTime = 0.15f;
    private float turnSmoothVelocity;

    private void Awake()
    {
        //_characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = horizontalInput * cam.transform.right + verticalInput* cam.transform.forward;
        
        transform.Translate (direction.normalized * speed * Time.deltaTime,Space.World);

       
        /*
        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f,angle,0f);

            //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //_characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        */
        
    }

}
