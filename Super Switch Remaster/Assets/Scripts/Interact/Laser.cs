using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : InteractObject
{
    private enum IsLookingAt
    {
        Right,
        Down
    }

    [SerializeField] private CanvasGroup laserVisual;
    [SerializeField] private Vector2 isLookingAtDirection;
    [SerializeField] private float distance = 100f;
    [SerializeField] private LayerMask targetMaskLaser;
    [SerializeField] private IsLookingAt isLookingAt;

    [SerializeField] private bool isActive = true;

    [SerializeField] private bool canBeManipulate = true;

    private void Start()
    {
        if (PlayerController.Instance != null && canBeManipulate)
        {
            PlayerController.Instance.OnInteract += PlayerController_OnInteract;
        }

        IsActiveLaser();

    }

    private void PlayerController_OnInteract(object sender, System.EventArgs e)
    {
        Interact();
    }

    private void FixedUpdate()
    {
        UpdateLaser();
    }

    private void UpdateLaser()
    {
        if (!isActive)
            return;

        switch (isLookingAt)
        {
            case IsLookingAt.Right:
                isLookingAtDirection = Vector2.right;
                break;
            case IsLookingAt.Down:
                isLookingAtDirection = Vector2.down;
                break;
            default:
                break;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, isLookingAtDirection, distance, targetMaskLaser);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                //Hacer efecto de zoom al personaje que murió y ahora si reiniciar despues de x segundos
                GameManager.Instance.RestartScene();
            }
        }
    }

    private IEnumerator UpdateLaserVisual()
    {
        yield return new WaitForSeconds(0);

        isActive = !isActive;

        IsActiveLaser();

        UpdateLaser();

    }

    private void IsActiveLaser()
    {
        if (isActive)
        {
            laserVisual.alpha = 1f;
        }
        else
        {
            laserVisual.alpha = 0.25f;
        }
    }

    public override void Interact()
    {
        StartCoroutine(UpdateLaserVisual());
    }
}
