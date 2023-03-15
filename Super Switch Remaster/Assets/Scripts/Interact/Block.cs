using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : InteractObject
{
    [SerializeField] private GameObject blockVisual;
    [SerializeField] private bool isActive = true;
    [SerializeField] private bool canBeManipulate = true;

    private void Start()
    {
        if (PlayerController.Instance != null && canBeManipulate)
        {
            PlayerController.Instance.OnInteract += PlayerController_OnInteract;
        }

        IsActiveBlock();

    }

    private void PlayerController_OnInteract(object sender, System.EventArgs e)
    {
        Interact();
    }

    private IEnumerator UpdateBlockVisual()
    {
        yield return new WaitForSeconds(0);

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
        blockVisual.SetActive(true);
    }

    private void Hide()
    {
        blockVisual.SetActive(false);
    }

    public override void Interact()
    {
        StartCoroutine(UpdateBlockVisual());
    }

}
