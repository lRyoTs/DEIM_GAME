using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [Header("References")]
    private PlayerControls _input;
    private PlayerController _playerController;

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private float normalSensitivity = 1f;
    [SerializeField] private float aimSensitivity = 0.5f;

    private Vector3 mouseWorldPosition;

    private void Awake()
    {
        _input = GetComponent<PlayerControls>();
        _playerController = GetComponent<PlayerController>();
    }


    // Update is called once per frame
    void Update()
    {
        GetAimDirection();
        SwitchToAimCamera();
        
    }

    /*
    private Vector3 GetMouseWorldPosition() {
        Vector3 vec = Get
    }
    */
    private void SwitchToAimCamera()
    {
        GetAimDirection();
        if (_input.Aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            _playerController.SetSensitivity(aimSensitivity);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            _playerController.SetSensitivity(normalSensitivity);
        }
    }

    private void GetAimDirection()
    {
        mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f); // Get center of the screen
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }

        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
    }

    public Vector3 GetMouseWorldPosition()
    {
        return mouseWorldPosition;
    }
}
