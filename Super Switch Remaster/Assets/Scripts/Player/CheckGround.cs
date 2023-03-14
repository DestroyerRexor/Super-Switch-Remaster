using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public event EventHandler OnGrounded;

    private bool isGrounded;

    public bool IsGrounded { get => isGrounded; private set => isGrounded = value; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Player") || other.CompareTag("Cube"))
        {
            IsGrounded = true;
            OnGrounded?.Invoke(this, EventArgs.Empty);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Player") || other.CompareTag("Cube"))
        {
            IsGrounded = true;
            OnGrounded?.Invoke(this, EventArgs.Empty);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Player") || other.CompareTag("Cube"))
        {
            IsGrounded = false;
            OnGrounded?.Invoke(this, EventArgs.Empty);
        }
    }
}
