using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string DIRECTION_X = "DirectionX";
    private const string JUMP = "Jump";
    private const string FALLING = "Falling";

    [SerializeField] private CheckGround ground;

    private Animator animator;

    private Vector2 direction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ground.OnGrounded += Ground_OnGrounded;
        PlayerController.Instance.OnFalling += Player_OnFalling;
    }

    private void Player_OnFalling(object sender, System.EventArgs e)
    {
        if (!ground.IsGrounded)
        {
            animator.SetTrigger(FALLING);
        }
    }

    private void Ground_OnGrounded(object sender, System.EventArgs e)
    {
        animator.SetBool(JUMP, !ground.IsGrounded);
    }

    public void SetInputVector(Vector2 direction)
    {
        this.direction = direction.normalized;

        animator.SetFloat(DIRECTION_X, Mathf.Abs(this.direction.x));
    }
}
