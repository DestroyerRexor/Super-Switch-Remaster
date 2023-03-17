using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance { get; private set; }

    public event EventHandler OnInteract;

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

    [Header("Wall Slide")]
    [SerializeField, Range(0.1f, 5f)] private float wallSlideMaxSpeed = 2f;
    [SerializeField, Range(0.05f, 0.5f)] private float wallStickTime = 0.25f;

    [Header("Wall Jump")]
    [SerializeField] private Vector2 wallJumpClimb = new Vector2(4f, 12f);
    [SerializeField] private Vector2 wallJumpBounce = new Vector2(10.7f, 10f);
    [SerializeField] private Vector2 wallJumpLeap = new Vector2(14f, 12f);

    [Header("Booleans")]
    private bool canMove;
    private bool canJump;
    private bool canRotate = true;
    private bool isJumping;
    private bool retrieveJumpInput;
    private bool onWall;
    private bool wallJumping;

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
    private float wallStickCounter;
    private float wallDirectionX;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CollisionDataRetriever collisionDataRetriever;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("PLAYER CONTROLLER SINGLETON - Trying to create another instance of singleton!!");
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

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

        velocity = rb.velocity;
        onWall = collisionDataRetriever.OnWall;
        wallDirectionX = collisionDataRetriever.ContactNormal.x;

        #region Wall Stick

        if (collisionDataRetriever.OnWall && !collisionDataRetriever.OnGround && !wallJumping)
        {
            if (wallStickCounter > 0)
            {
                velocity.x = 0;
                if (input.x == collisionDataRetriever.ContactNormal.x)
                {
                    wallStickCounter -= Time.deltaTime;
                }
                else
                {
                    wallStickCounter = wallStickTime;
                }
            }
            else
            {
                wallStickCounter = wallStickTime;
            }
        }

        #endregion

        #region Wall Slide

        if (onWall)
        {
            if (velocity.y < -wallSlideMaxSpeed)
            {
                velocity.y = -wallSlideMaxSpeed;
            }
        }
        #endregion

        #region Wall Jump

        if ((onWall && velocity.x == 0) || collisionDataRetriever.OnGround)
        {
            wallJumping = false;
        }

        if (desiredJump)
        {
            if (-wallDirectionX == input.x)
            {
                velocity = new Vector2(wallJumpClimb.x * wallDirectionX, wallJumpClimb.y);
                wallJumping = true;
                desiredJump = false;
            }
            else if (input.x == 0)
            {
                velocity = new Vector2(wallJumpBounce.x * wallDirectionX, wallJumpBounce.y);
                wallJumping = true;
                desiredJump = false;
            }
            else
            {
                velocity = new Vector2(wallJumpLeap.x * wallDirectionX, wallJumpLeap.y);
                wallJumping = true;
                desiredJump = false;
            }
        }
        #endregion

        #region Controlar caida y fases de salto
        if (collisionDataRetriever.OnGround && (rb.velocity.y > -0.01f && rb.velocity.y < 0.01f))
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
        }
        else
        {
            rb.gravityScale = defaultGravityScale;
        }
        #endregion

        rb.velocity = velocity;

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (canMove)
        {
            if (input.x > 0.3f)
            {
                acceleration = collisionDataRetriever.OnGround ? maxAcceleration : maxAirAcceleration;
                maxSpeedChange = acceleration * Time.deltaTime;
                velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

                rb.velocity = velocity;

                if (canRotate)
                {
                    transform.rotation = input.x > 0.3f ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
                }


                if (!collisionDataRetriever.OnGround)
                    return;

            }
            else if (input.x < -0.3f)
            {
                acceleration = collisionDataRetriever.OnGround ? maxAcceleration : maxAirAcceleration;
                maxSpeedChange = acceleration * Time.deltaTime;
                velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

                rb.velocity = velocity;
                if (canRotate)
                {
                    transform.rotation = input.x < -0.3f ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
                }

                if (!collisionDataRetriever.OnGround)
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
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rb.velocity.y);
            }

            velocity.y += jumpSpeed;

            rb.velocity = velocity;
        }
    }

    private void HandleInput()
    {
        desiredVelocity = new Vector2(input.x, 0f) * Mathf.Max(maxSpeed - collisionDataRetriever.GetFriction(), 0f);

        if (onWall && !collisionDataRetriever.OnGround)
        {
            desiredJump |= retrieveJumpInput;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionDataRetriever.EvaluateCollision(collision);

        if (collisionDataRetriever.OnWall && !collisionDataRetriever.OnGround && wallJumping)
        {
            rb.velocity = Vector2.zero;
        }

    }

    public void SetInputVector(Vector2 direction)
    {
        input = direction.normalized;
    }

    public void SetJumpInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed && canJump)
        {
            context.action.started += Action_started;
            context.action.canceled += Action_canceled;

            Jump();

        }
    }

    private void Action_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        retrieveJumpInput = true;
    }

    private void Action_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        retrieveJumpInput = false;
    }

    public void SetInteract(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        context.action.started += OnInteract_started;
        context.action.canceled += OnInteract_canceled;
    }

    private void OnInteract_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteract_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }
}
