using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public Vector2 Move { get; private set;}
    public Vector2 Look { get; private set;}
    public bool Jump {get ; set;}
    public bool Interact { get; set;}
    public bool Shoot { get; set;}
    public bool Dash {  get; set;}
    public bool Aim { get; set; }

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnInteract(InputValue value) 
    {
        InteractInput(value.isPressed);   
    }

    public void OnShoot(InputValue value) {
        ShootInput(value.isPressed);
    }

    public void OnDash(InputValue value)
    {
        DashInput(value.isPressed);
    }

    public void OnAim(InputValue value)
    {
        AimInput(value.isPressed);
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        Move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        Look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        Jump = newJumpState;
    }

    public void InteractInput(bool newInteractsState)
    {
        Interact = newInteractsState;
    }

    public void ShootInput(bool newShootState) {
        Shoot = newShootState;
    }

    public void DashInput(bool newDashState)
    {
        Dash = newDashState;
    }

    public void AimInput(bool newAimState)
    {
        Aim = newAimState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
