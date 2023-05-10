using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Camera playerCamera;
    Transform groundCheck;
    LayerMask groundLayer;
    float mouseSens { get; set; } = 400f;
    float xRotation { get; set; } = 0f;
    [SerializeField]
    bool isGrounded = true;
    [SerializeField]
    bool isJumping = false;
    Vector3 jumpVelo;
    float sideToSide;
    float fowardAndBack;

    //ststs
    float moveSpeed = 6f;
    float gravity = -21f;
    float normalGravity;
    float strafeGravity = -2f;
    float jumpHeight = 1.5f;
    float dashSpeed;

    public void Start()
    {
        normalGravity = gravity;
        dashSpeed = 1.1f * moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Update()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, .4f, groundLayer);
        Movement();
        MouseMovement();
    }
    public void Movement()
    {
        sideToSide = Input.GetAxis("Horizontal");
        fowardAndBack = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(sideToSide, 0, fowardAndBack) * moveSpeed * Time.deltaTime);
    }
    public void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        this.gameObject.transform.Rotate(Vector3.up, mouseX);
    }
}
