using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    enum MovementType {
        FieldMovement,
        BattleMovement
    }

    private float horizontalInput;
    private float verticalInput;
    

    private MovementType currentMovementType;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        switch (currentMovementType) {
            case MovementType.FieldMovement:
                HandleFieldMovement();
                break;
             case MovementType.BattleMovement:
                HandleBattleMovement();
                break;
        }
    }

    private void HandleFieldMovement() {
         
    }

    private void HandleBattleMovement() {
    
    }
}
