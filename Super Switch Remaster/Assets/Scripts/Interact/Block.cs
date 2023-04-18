using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Block : InteractObject
{
    [SerializeField] private SpriteRenderer blockVisual;
    [SerializeField] private bool isActive;
    [SerializeField] private bool canBeManipulate = true;

    private void Start()
    {
        if (PlayerController.Instance != null && canBeManipulate)
        {
            PlayerController.Instance.OnInteract += PlayerController_OnInteract;
        }

        IsActiveBlock();
    }

    private void OnDestroy()
    {
        PlayerController.Instance.OnInteract -= PlayerController_OnInteract;
    }

    private void PlayerController_OnInteract(object sender, System.EventArgs e)
    {
        Interact();
    }

    private void UpdateBlockVisual()
    {
        isActive = !isActive;

        IsActiveBlock();
    }

    private void IsActiveBlock()
    {
        if (isActive)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        blockVisual.color = new Color(blockVisual.color.r, blockVisual.color.g, blockVisual.color.b, 1f);
        blockVisual.GetComponent<Collider2D>().isTrigger = false;
    }

    private void Hide()
    {
        blockVisual.color = new Color(blockVisual.color.r, blockVisual.color.g, blockVisual.color.b, 0.5f);
        blockVisual.GetComponent<Collider2D>().isTrigger = true;
    }

    public override void Interact()
    {
        UpdateBlockVisual();
    }

}
