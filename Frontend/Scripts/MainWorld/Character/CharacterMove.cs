using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 200.0f;
    public float jumpForce = 8.0f;
    public float sprintMultiplier = 2.0f;

    private CharacterController controller;
    private Vector3 velocity;
    private Animator animator;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement *= sprintMultiplier;
        }

        if (controller.isGrounded)
        {
            velocity.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = jumpForce;
            }
        }

        velocity.x = movement.x * moveSpeed;
        velocity.z = movement.z * moveSpeed;
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Update Animator parameters
        int movementState = 0; // 0: Idle, 1: Walking, 2: Running, 3: Jumping, 4: Defat
        if (controller.isGrounded)
        {
            if (controller.velocity.magnitude > 0.1f)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    movementState = 2; // Running
                }
                else
                {
                    movementState = 1; // Walking
                }
            }
            //else
            //{
            //    movementState = 4; // Add a condition for Defat animation
            //}
        }
        // else
        // {
        //     movementState = 3; // Jumping
        // }
        animator.SetInteger("movementState", movementState);
    }
    // public float moveSpeed = 5.0f;
    // public float rotationSpeed = 200.0f;
    // public float jumpForce = 8.0f;
    // public float sprintMultiplier = 2.0f;

    // private CharacterController controller;
    // private Vector3 velocity;
    // private Animator animator;

    // private void Start()
    // {
    //     controller = GetComponent<CharacterController>();
    //     animator = GetComponent<Animator>();
    // }

    // private void Update()
    // {
    //     float moveHorizontal = Input.GetAxis("Horizontal");
    //     float moveVertical = Input.GetAxis("Vertical");

    //     Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
    //     movement = Camera.main.transform.TransformDirection(movement);
    //     movement.y = 0;

    //     if (movement != Vector3.zero)
    //     {
    //         Quaternion targetRotation = Quaternion.LookRotation(movement);
    //         transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //     }

    //     if (Input.GetKey(KeyCode.LeftShift))
    //     {
    //         movement *= sprintMultiplier;
    //     }

    //     if (controller.isGrounded)
    //     {
    //         velocity.y = 0;

    //         if (Input.GetButtonDown("Jump"))
    //         {
    //             velocity.y = jumpForce;
    //         }
    //     }

    //     velocity.x = movement.x * moveSpeed;
    //     velocity.z = movement.z * moveSpeed;
    //     velocity.y += Physics.gravity.y * Time.deltaTime;
    //     controller.Move(velocity * Time.deltaTime);

    //     // Update Animator parameters
    //     bool isMoving = controller.velocity.magnitude > 0.1f;
    //     animator.SetBool("isIdle", !isMoving && controller.isGrounded);
    //     animator.SetBool("isWalking", isMoving && !Input.GetKey(KeyCode.LeftShift));
    //     animator.SetBool("isRunning", isMoving && Input.GetKey(KeyCode.LeftShift));
    //     //animator.SetBool("isDefat", /* Add a condition for Defat animation */);
    //     animator.SetBool("isJumping", !controller.isGrounded);
    // }
}
