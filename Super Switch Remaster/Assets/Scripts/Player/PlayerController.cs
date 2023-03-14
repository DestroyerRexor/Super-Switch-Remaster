using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance { get; private set; }

    public event EventHandler OnInteract;
    public event EventHandler OnFalling;

    [Header("Move Settings")]
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4.5f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 60f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 60f;

    [Header("Jump Settings")]
    [SerializeField, Range(0, 15f)] private float jumpHeight = 4.8f;
    [SerializeField, Range(0, 10)] private int maxAirJumps = 1;
    [SerializeField, Range(0, 10f)] private float downwardMovementMultiplier = 4.06f;
    [SerializeField, Range(0, 10f)] private float upwardMovementMultiplier = 4.16f;
    [SerializeField, Range(0, 0.3f)] private float coyoteTime = 0.2f;
    [SerializeField, Range(0, 0.3f)] private float jumpBufferTime = 0.2f;

    [Header("Booleans")]
    private bool canMove;
    private bool canJump;
    private bool canRotate = true;
    private bool isJumping;
    private bool retrieveJumpInput;

    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Vector2 input;

    [Header("Jump and movement components")]
    private bool desiredJump;
    private int jumpPhase;
    private float maxSpeedChange;
    private float acceleration;
    private float defaultGravityScale;
    private float coyoteCounter;
    private float jumpBufferCounter;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CheckGround ground;

    [Header("Friction")]
    private float friction;
    private PhysicsMaterial2D material;

    private void Awake()
    {
        Instance = this;

        defaultGravityScale = 1.5f;

        canMove = true;
        canJump = true;
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        #region Controlar caida y fases de salto
        velocity = rb.velocity;

        if (ground.IsGrounded && (rb.velocity.y > -0.01f && rb.velocity.y < 0.01f))
        {
            jumpPhase = 0;
            coyoteCounter = coyoteTime;
            isJumping = false;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (desiredJump)
        {
            desiredJump = false;

            jumpBufferCounter = jumpBufferTime;
        }
        else if (!desiredJump && jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0)
        {
            Jump();
        }

        if (rb.velocity.y > 0)
        {
            rb.gravityScale = upwardMovementMultiplier;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = downwardMovementMultiplier;
            OnFalling?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            rb.gravityScale = defaultGravityScale;
        }

        rb.velocity = velocity;
        #endregion

        HandleMovement();

    }

    private void HandleMovement()
    {
        if (canMove)
        {
            if (input.x > 0.3f)
            {
                acceleration = ground.IsGrounded ? maxAcceleration : maxAirAcceleration;
                maxSpeedChange = acceleration * Time.deltaTime;
                velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

                rb.velocity = velocity;

                if (canRotate)
                {
                    transform.rotation = input.x > 0.3f ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
                }


                if (!ground.IsGrounded)
                    return;

            }
            else if (input.x < -0.3f)
            {
                acceleration = ground.IsGrounded ? maxAcceleration : maxAirAcceleration;
                maxSpeedChange = acceleration * Time.deltaTime;
                velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

                rb.velocity = velocity;
                if (canRotate)
                {
                    transform.rotation = input.x < -0.3f ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
                }

                if (!ground.IsGrounded)
                    return;

            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    private void Jump()
    {
        if (coyoteCounter > 0f || (jumpPhase < maxAirJumps && isJumping))
        {
            if (isJumping)
            {
                jumpPhase += 1;
            }

            jumpBufferCounter = 0;
            coyoteCounter = 0;

            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);

            isJumping = true;

            if (velocity.y != 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;

            rb.velocity = velocity;

        }
    }

    private void HandleInput()
    {
        desiredVelocity = new Vector2(input.x, 0f) * Mathf.Max(maxSpeed - GetFriction(), 0f);

        desiredJump |= retrieveJumpInput;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RetrieveFriction(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        RetrieveFriction(collision);
    }
    private void OnCollisionExit2D()
    {
        friction = 0;
    }

    private void RetrieveFriction(Collision2D collision)
    {
        if (collision.rigidbody.sharedMaterial != null)
        {
            material = collision.rigidbody.sharedMaterial;

            friction = 0;

            if (material != null)
            {
                friction = material.friction;
            }
        }
    }

    private float GetFriction() { return friction; }

    public void ChangeGravity()
    {
        var defaultDownwardMultiplier = 7.4f;

        StartCoroutine(ReturnGravity(defaultDownwardMultiplier));
    }

    IEnumerator ReturnGravity(float defaultMultiplier)
    {
        var timeElapsed = 0f;
        var lerpDuration = 0.75f;
        while (timeElapsed < lerpDuration)
        {
            downwardMovementMultiplier = Mathf.Lerp(0, defaultMultiplier, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        downwardMovementMultiplier = defaultMultiplier;
    }

    public void SetInputVector(Vector2 direction)
    {
        input = direction.normalized;
    }

    public void SetJumpInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed && canJump)
        {
            Jump();
        }
    }

    public void SetInteract(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed && true)
        {
            OnInteract?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetRetrieveJumpInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed && canJump)
        {
            retrieveJumpInput = context.action.triggered;
        }
    }

}
