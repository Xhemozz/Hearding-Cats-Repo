
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private PlayerInputController inputController;
    private Rigidbody rb;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform ground;
    [SerializeField] private LayerMask groundLayer;

    private float moveX;
    private float moveY;

    private bool isJumping;
    private bool isGrounded;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        inputController = GetComponent<PlayerInputController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        inputController.OnJumpPressed += JumpPressed;
    }

    private void Update()
    {
        moveX = inputController.MovementInputVector.x;
        moveY = inputController.MovementInputVector.y;
    }
    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(ground.position, 0.2f, groundLayer);

        if (inputController.MovementInputVector != Vector2.zero)
        {
            animator.SetTrigger("Move");
            MovePlayer();
        }
        if (isJumping && isGrounded)
        {
            Debug.Log($"Grounded {isGrounded}");
            Jump();
            isJumping = false;
        }

    }

    private void MovePlayer()
    {
        Vector3 direction = transform.right * moveX + transform.forward * moveY;
        direction.Normalize();
        rb.AddForce(direction * speed, ForceMode.Impulse);
        RotatePlayer(direction);
    }

    // ChatGPT fixed the RotatePlayer method. Dunno how but it works now.
    private void RotatePlayer(Vector3 moveDir)
    {
        if (inputController.MovementInputVector.y > 0.1f ||
            Mathf.Abs(inputController.MovementInputVector.x) > 0.1f)
        {
            Vector3 forwardDirection = moveDir;
            forwardDirection.y = 0f;

            if (forwardDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(forwardDirection);
                Quaternion newRotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                );

                rb.MoveRotation(newRotation);
            }
        }
    }
    private void JumpPressed()
    {
        isJumping = true;
    }
    private void Jump()
    {
        animator.SetTrigger("Jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
