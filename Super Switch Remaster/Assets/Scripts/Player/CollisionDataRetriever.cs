using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDataRetriever : MonoBehaviour
{
    public event EventHandler OnGrounded;
    public event EventHandler OnSliding;

    public bool OnGround { get; private set; }
    public bool OnWall { get; private set; }

    public Vector2 ContactNormal { get; private set; }


    [Header("Friction")]
    private float friction;
    private PhysicsMaterial2D material;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        if (!OnGround)
        {
            OnGrounded?.Invoke(this, EventArgs.Empty);
        }
        OnWall = false;
        if (!OnWall)
        {
            OnSliding?.Invoke(this, EventArgs.Empty);
        }
        friction = 0;
    }

    public void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactNormal = collision.GetContact(i).normal;
            OnGround |= Mathf.Abs(ContactNormal.y) >= 0.9f;
            if (OnGround)
            {
                OnGrounded?.Invoke(this, EventArgs.Empty);
            }
            OnWall = Mathf.Abs(ContactNormal.x) >= 0.9f;
            if (OnWall)
            {
                OnSliding?.Invoke(this, EventArgs.Empty);
            }
        }
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

    public float GetFriction() { return friction; }

}
