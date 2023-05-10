using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Camera playerCamera;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public CharacterController characterController;
    public float mouseSens { get; set; } = 400f;
    public float xRotation { get; set; } = 0f;
    public bool isGrounded = true;
    public bool isJumping = false;
    public Vector3 jumpVelo;
    public float sideToSide;
    public float fowardAndBack;
    public Vector3 movementVector = Vector3.zero;

    //ststs
    public float moveSpeed = 6f;
    public float gravity = -21f;
    public float normalGravity;
    public float jumpHeight = 1.5f;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController.GetComponent<CharacterController>();
    }
    public void Update()
    {
        //Player Movement
        isGrounded = Physics.CheckSphere(groundCheck.position, .4f, groundLayer);
        Movement();
        MouseMovement();
        Jumping();
    }
    //Player Movement
    public void Movement()
    {
        sideToSide = Input.GetAxis("Horizontal");
        fowardAndBack = Input.GetAxis("Vertical");
        movementVector = transform.right * sideToSide + transform.forward * fowardAndBack;
        characterController.Move(movementVector * moveSpeed * Time.deltaTime);

    }
    //Camera Movement
    public void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        this.gameObject.transform.Rotate(Vector3.up, mouseX);
    }
    //Jumping
    public void Jumping()
    {
        bool jump = Input.GetKeyDown(KeyCode.Space);
        if(jump && isGrounded)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
        if(isGrounded && jumpVelo.y < 0)
        {
            jumpVelo.y = -2f;
        }
        if (isJumping)
        {
            jumpVelo.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        jumpVelo.y += gravity * Time.deltaTime;
        characterController.Move(jumpVelo * Time.deltaTime);
    }
}
