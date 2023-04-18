using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string DIRECTION_X = "DirectionX";
    private const string IS_JUMP = "IsJumping";
    private const string IS_SLIDING = "IsSliding";

    [SerializeField] private CollisionDataRetriever collisionDataRetriever;

    private Animator animator;

    private Vector2 direction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        collisionDataRetriever.OnGrounded += CollisionDataRetriever_onGrounded;
        collisionDataRetriever.OnSliding += CollisionDataRetriever_onSliding;

        GameManager.Instance.OnSceneChanged += GameManager_onSceneChanged;
    }

    private void OnDestroy()
    {
        collisionDataRetriever.OnGrounded -= CollisionDataRetriever_onGrounded;
        collisionDataRetriever.OnSliding -= CollisionDataRetriever_onSliding;

        GameManager.Instance.OnSceneChanged -= GameManager_onSceneChanged;
    }

    private void GameManager_onSceneChanged(object sender, System.EventArgs e)
    {
        if (animator != null)
        {
            animator.SetBool(IS_SLIDING, false);
            animator.SetFloat(DIRECTION_X, 0);
        }
    }

    private void CollisionDataRetriever_onSliding(object sender, System.EventArgs e)
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

    private void CollisionDataRetriever_onGrounded(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_JUMP, !collisionDataRetriever.OnGround);
    }

    public void SetInputVector(Vector2 direction)
    {
        this.direction = direction.normalized;

        animator.SetFloat(DIRECTION_X, Mathf.Abs(this.direction.x));
    }
}
