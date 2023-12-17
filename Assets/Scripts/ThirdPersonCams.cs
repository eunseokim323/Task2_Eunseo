using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCams : MonoBehaviour
{
    [Header("References")] public Transform orientation;
    public PlayerMoveScript playerMoveScript;
    public GameObject thirdPersonCamera;
    public Transform _player;
    public Transform _playerObj;
    public Rigidbody _rb;

    public float rotationSpeed;
    
    // Cursor Lock?
    public bool _isCursorLock;

    [SerializeField] private GameObject inputField;

    private void Start()
    {
        
        Cursor.lockState = _isCursorLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = _isCursorLock;
    }

    public void CursorOn(bool isOn)
    {
        Cursor.lockState = _isCursorLock ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOn;
        playerMoveScript.enabled = !isOn;
    }

    private void Update()
    {
        
        Vector3 viewDir = _player.position - new Vector3(transform.position.x, 
            _player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (inputDir != Vector3.zero)
        {
            _playerObj.forward = Vector3.Slerp(_playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
