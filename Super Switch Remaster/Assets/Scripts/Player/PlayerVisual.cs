using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string DIRECTION_X = "DirectionX";
    private const string IS_JUMP = "IsJumping";
    private const string IS_SLIDING = "IsSliding";
    private const string RUN_AND_JUMP = "RunNJump";

    [SerializeField] private CollisionDataRetriever collisionDataRetriever;

    private Animator animator;

    private Vector2 direction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        collisionDataRetriever.OnGrounded += CollisionDataRetriever_OnGrounded;
        collisionDataRetriever.OnSliding += CollisionDataRetriever_OnSliding;
    }

    private void CollisionDataRetriever_OnSliding(object sender, System.EventArgs e)
    {
        if (!collisionDataRetriever.OnGround)
        {
            animator.SetBool(IS_SLIDING, collisionDataRetriever.OnWall);
        }
        else
        {
            animator.SetBool(IS_SLIDING, false);
        }
    }

    private void CollisionDataRetriever_OnGrounded(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_JUMP, !collisionDataRetriever.OnGround);
    }

    public void SetInputVector(Vector2 direction)
    {
        this.direction = direction.normalized;

        animator.SetFloat(DIRECTION_X, Mathf.Abs(this.direction.x));
    }
}
