using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    [Header("Movement")]
    // variables related to the player's movement.
    public float moveSpeed; 
    public float groundDrag; 
    
    public float jumpForce;
    public float jumpCooldown; 
    public float airMultiplier; 
    bool readyToJump; 
    [HideInInspector] public float walkSpeed; 
    [HideInInspector] public float sprintSpeed; 
    
    [Header("Keybinds")]
    // 게임에서 사용하는 각종 기능에 대한 키 설정
    public KeyCode jumpKey = KeyCode.Space; 
    
    [Header("Ground Check")]

    public float playerHeight; 
    public LayerMask whatIsGround; 
    public bool grounded; 
    
    public Transform orientation; 
    
    float horizontalInput; 
    float verticalInput; 
    
    Vector3 moveDirection; 
    
    Rigidbody rb; // Rigidbody 
    private Animator _animator; // animator
    private CameraManager _cameraManager;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        rb.freezeRotation = true; 
        _cameraManager = FindFirstObjectByType<CameraManager>();
    
        readyToJump = true; 
    }
    
    private void Update()
    {
        if (!_cameraManager.isTeacher)
        {
        
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
    
            MyInput(); 
            SpeedControl(); 
    
            // 걷는 애니메이션
            _animator.SetBool("isMove", moveDirection != Vector3.zero);
            _animator.SetBool("FreeFall", !grounded);
            _animator.SetBool("Grounded", grounded);
        }
    
        // 드래그 처리
        if (grounded)
            rb.drag = groundDrag; 
        else
            rb.drag = 0; 
    }
    
    private void FixedUpdate()
    {
        if (!_cameraManager.isTeacher)
        {
            MovePlayer(); 
        }
    }
    
    
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); 
        verticalInput = Input.GetAxisRaw("Vertical"); 
    
        
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false; 
    
            Jump(); 
    
            Invoke(nameof(ResetJump), jumpCooldown); 
        }
    }
    
    // player movement
    private void MovePlayer()
    {
        
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    
       
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); 
    
        
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); 
    }
    

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    
       
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    
  
    private void Jump()
    {
        
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        _animator.SetTrigger("JumpTrigger");
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); 
    }
    

    private void ResetJump()
    {
        readyToJump = true; 
    }
    
   
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.3f), Color.black); 
    }

}
